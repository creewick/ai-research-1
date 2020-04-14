const canvas = document.getElementById("field");
const con = document.getElementById("console");
const ctx = canvas.getContext('2d');

let cellSize = 8;
let index = 0;
let consoleOut = "";
let showTrajectories = true;

document.body.onkeydown = keydown;
canvas.onclick = click;

function click() {
  showTrajectories = !showTrajectories;
  update();
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
    index = data[1].length-1;
  else return;
  ev.preventDefault();
  update();
}

function rewind(dir){
  index = Math.min(data[1].length-1, Math.max(0, index+dir));
}

function update() {
  drawEvent(index);
}

function drawEvent(index){
  const state = data[1][index]
  adjustScale(state);
  clearSpace();
  drawFlags(data[0][3], state[0]);
  drawObstacles(data[0][4]);
  consoleOut = `Time: ${index} FlagsTaken: ${state[0]} Countdown: ${state[1]}\n`;
  drawCar(state[2]);
  drawCar(state[3]);
  drawLines(data[2][index], state);
  con.innerText = consoleOut;
}

function drawLines(solutions, state){
  if (!solutions) return;
  const lastSolution = solutions[solutions.length - 1];
  logMoves(lastSolution[0]);
  logMoves(lastSolution[1]);

  if (!showTrajectories) return;
  drawLinesCar(solutions, state[2], x => x[0]);
  drawLinesCar(solutions, state[3], x => x[1]);
}

function logMoves(moves){
  for (let i = 0; i < moves.length; i++){
    consoleOut += `(${moves[i].join(',')})`
  }
  consoleOut += '\n';
}

function drawLinesCar(solutions, car, getMoves){
  for (let i = 0; i < solutions.length; i++){
    ctx.strokeStyle = (i < solutions.length - 1) ? 'rgba(255,0,0,.5)' : 'rgb(255,255,0)';
    let pos = [car[0], car[1]];
    let v = [car[2], car[3]];
    const moves = getMoves(solutions[i]);
    const points = [pos];

    for (let j = 0; j < moves.length; j++){
      v = [v[0] + moves[j][0], v[1] + moves[j][1]]
      pos = [pos[0] + v[0], pos[1] + v[1]]
      points.push(pos);
    }
    ctx.stroke(createLine(points, 0.5));
  }
}


function drawFlags(flags, taken){
  ctx.strokeStyle = 'rgba(50,50,50,1)';
  ctx.stroke(createLine([...flags, flags[0]]));
  for (let i = 0; i < flags.length; i++){
    let isNext = i === taken % flags.length;
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
  ctx.fillStyle = car[4] ? 'green' : 'red';
  ctx.fill(createDisk(car[0], car[1], 2));
  consoleOut += `Pos: (${car[0]},${car[1]}) V: (${car[2]},${car[3]}) Alive:${car[4]}\n`;
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

function adjustScale(state) {
  let objects = [
      ...data[0][3],
      ...data[0][4],
         state[2],
         state[3]
  ];
  cellSize = 8;
  while (cellSize > 0.2 && objects.some(p => !fitInScreen(p)))
    cellSize/=1.2;
}
