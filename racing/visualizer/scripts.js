const canvas = document.getElementById("field");
const con = document.getElementById("console");
const ctx = canvas.getContext('2d');

let cellSize = 8;
let index = 0;
let log = null;
let consoleOut = "";
let showTrajectories = true;
const url = new URL(document.location.href);

document.body.onkeydown = keydown;
canvas.onclick = click;

let db;
let logsRef;
try {
    const firebaseConfig = {
        apiKey: "AIzaSyBO8QoDZknDF3XY1dw_tXeIVbPsvDiBIdw",
        authDomain: "game-ai-logs.firebaseapp.com",
        databaseURL: "https://game-ai-logs.firebaseio.com",
        projectId: "game-ai-logs",
        storageBucket: "game-ai-logs.appspot.com",
        messagingSenderId: "655269759371",
        appId: "1:655269759371:web:b8baab1eebefaf2617078e"
    };
    firebase.initializeApp(firebaseConfig);
    db = firebase.firestore();

    const storage = firebase.storage();
    logsRef = storage.ref().child('logs');
    firebase.auth().onAuthStateChanged(function (user) {
        loadApp().catch(console.log);
    });
} catch (e) {
    console.log(e);
    loadApp().catch(console.log);
}

function click() {
    showTrajectories = !showTrajectories;
    update();
}

function handleFirebaseError(e){
    console.log(e);
    alert(e.message);
}

async function loadLog(logId, ver) {
    if (ver === "2") {
        const url = await logsRef.child(logId + ".json").getDownloadURL();
        const resp = await fetch(url);
        if (!resp.ok)
            throw new Error("Can't fetch log " + logId + " " + resp);
        return await resp.json();
    } else {
        const doc = await db.collection("logs").doc(logId).get();
        if (!doc.exists) throw new Error("Log not found");
        return doc.data();
    }
}
async function loadApp(){
    let logId = url.searchParams.get("logId");
    let version = url.searchParams.get("v");
    if (logId){
        window.originalLog = null;
        await auth();
        try {
            const data = await loadLog(logId, version);
            log = convertGameLog(JSON.parse(data.log));
            document.getElementById("replayCreationTime").innerText = data.creationTime;
            document.getElementById("replayAuthor").innerText = data.authorEmail;
            index = +url.searchParams.get("t");
            update();
        }
        catch(e) {
            handleFirebaseError(e);
        }
    }
    else {
        log = convertGameLog(originalLog);
        index = +url.searchParams.get("t");
        document.getElementById("sharelink").style.display = "block";
        update();
    }
}

function auth() {
    let auth = window.firebase.auth();
    console.log(auth.currentUser);
    if (auth.currentUser) return null;
    var provider = new window.firebase.auth.GoogleAuthProvider();
    return auth.signInWithRedirect(provider);    
}

async function shareLink(){
    await auth();
    const logData = JSON.stringify(originalLog);
    const logId = window.md5(logData);
    try {
        await logsRef.child(logId + ".json").putString(JSON.stringify(
            {
            log: logData,
            creationTime: new Date().toString(),
            authorEmail: window.firebase.auth().currentUser.email
            }));
    } 
    catch (e){
        handleFirebaseError(e);
        return;
    }
    const url = new URL("https://game-ai-logs.firebaseapp.com/");
    url.search = "logId=" + logId + "&v=2";
    const link = document.getElementById("link");
    link.innerText = link.href = url.href;
    document.getElementById("sharelink").style.display = "none";
}

function keydown(ev){
    const code = ev.code;
    console.log(code);
    if (code === "ArrowRight" || code === "KeyD")
        rewind(1);
    else if (code === "ArrowLeft" || code === "KeyA")
        rewind(-1);
    else if (code === "Home" || code === "KeyQ")
        index = 0;
    else if (code === "End" || code === "KeyE")
        index = log.ticks.length-1;
    else return;
    ev.preventDefault();
    update();
}

function convertGameLog(data) {
    return {
        raceDuration: data[0],
        flags: data[1],
        obstacles: data[2],
        ticks: data[3].map(convertTick)
    };
}

function convertTick(data){
    return {
        time: data[0],
        isFinished: data[1],
        car: convertCar(data[2])
    };
}

function convertCar(data) {
    return {
        pos: data[0],
        v: data[1],
        radius: data[2],
        flagsTaken: data[3],
        isAlive: data[4],
        nextCommand: data[5],
        debugOutput: data[6],
        debugLines: data[7].map(convertLine)
    };
}

function convertLine(data) {
    return {
        intensity: data[0],
        points: data.slice(1)
};
}

function rewind(dir){
    index = Math.min(log.ticks.length-1, Math.max(0, index+dir));
}

function update() {
    drawEvent(log.ticks[index]);
}

function drawEvent(tick){
    const car = tick.car;
    adjustScale(car);
    clearSpace();
    drawFlags(log.flags, car);
    drawObstacles(log.obstacles);
    consoleOut = tick.time + "\n";
    drawCar(car);
    con.innerText = consoleOut;
}

function drawFlags(flags, car){
    ctx.strokeStyle = 'rgba(50,50,50,1)';
    ctx.stroke(createLine([...flags, flags[0]]));
    for (let i = 0; i < flags.length; i++){
        let isNext = i === car.flagsTaken % flags.length;
        drawFlag(flags[i], i, isNext);
    }
}


function drawObstacles(obstacles){
    for(let i = 0; i < obstacles.length; i++){
        drawObstacle(obstacles[i]);
    }
}

function drawObstacle(o) {
    ctx.fillStyle = 'grey';
    ctx.fill(createDisk(...o));
}

function drawFlag(flag, index, isNext) {
    ctx.fillStyle = isNext ? 'cyan' : 'blue';
    ctx.fill(createDisk(flag[0], flag[1]));
}

function drawCar(car) {
    const pos = car.pos;
    ctx.fillStyle = car.isAlive ? 'green' : 'red';
    ctx.fill(createDisk(pos[0], pos[1], car.radius));
    consoleOut += serializeCar(car) + "\n" + car.debugOutput;
    if (showTrajectories) {
        for (let line of car.debugLines) {
            const r = Math.round(255 * 1 - line.intensity);
            const g = Math.round(255 * line.intensity);
            ctx.strokeStyle = `rgba(${r},${g},0)`;
            console.log((r, g));
            ctx.stroke(createLine(line.points));
        }
    }
}

function serializeCar(car){
    return `${car.pos[0]},${car.pos[1]} ${car.v[0]},${car.v[1]} ${car.radius}`;
}

function createLine(points){
    const res = new Path2D();
    if (points.length > 0) {
        const start = points[0];
        res.moveTo(toScreenX(start[0]), toScreenY(start[1]));
        for (let p of points)
            res.lineTo(toScreenX(p[0]), toScreenY(p[1]));
    }
    return res; 
}

function clearSpace() {
    ctx.clearRect(0,0,canvas.width, canvas.height);
}

function toScreenX(gameX){
    const originX = canvas.width / 2 - cellSize / 2;
    return originX + gameX*cellSize;
}

function toScreenY(gameY){
    const originY = canvas.height / 2 - cellSize / 2;
    return originY + gameY*cellSize;
}

function createDisk(gameX, gameY, radius = 0) {
    const res = new Path2D();
    let x = toScreenX(gameX);
    let y = toScreenY(gameY);
    let size = radius*cellSize;
    if (size < 4)
        size = 4;
    res.arc(x, y, size, 0, 2*Math.PI);
    return res;
}

function createRect(gameX, gameY, radius = 0) {
    const res = new Path2D();
    let size = (2*radius+1)*cellSize;
    let left = toScreenX(gameX-radius);
    let top = toScreenY(gameY-radius);
    const minSize = 4;
    if (size < minSize){
        left -= (minSize-size)/2;
        top -= (minSize-size)/2;
        size = minSize;
    }
    res.rect(left, top, size, size);
    return res;
}

function fitInScreen(pos){
    const screenX = toScreenX(pos[0]);   
    const screenY = toScreenY(pos[1]);
    return screenX > 0 && screenX < canvas.width && screenY > 0 && screenY < canvas.height;
}

function adjustScale(car) {
    let objects = [car.pos, ...log.flags, ...log.obstacles];
    //cellSize = 8;
    while (cellSize > 0.2 && objects.some(p => !fitInScreen(p)))
        cellSize/=1.2;
}
