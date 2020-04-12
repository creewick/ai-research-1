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
    index = race.States.length-1;
  else return;
  ev.preventDefault();
  update();
}

function rewind(dir){
  index = Math.min(race.States.length-1, Math.max(0, index+dir));
}

function update() {
  drawEvent(index);
}

function drawEvent(index){
  const state = race.States[index]
  adjustScale(state);
  clearSpace();
  drawFlags(race.Track.Flags, state.FlagsTaken);
  drawObstacles(race.Track.Obstacles);
  consoleOut = index + "\n";
  drawCar(state, "FirstCar");
  drawCar(state, "SecondCar");
  drawLines(race.Solutions[index], state);
  con.innerText = consoleOut;
}

function drawLines(solutions, state){
  if (!solutions) return;
  const lastSolution = solutions[solutions.length - 1];
  logMoves("FirstCarMoves", lastSolution);
  logMoves("SecondCarMoves", lastSolution);

  if (!showTrajectories) return;
  drawLinesCar(solutions, state.FirstCar, x => x.FirstCarMoves);
  drawLinesCar(solutions, state.SecondCar, x => x.SecondCarMoves);
}

function logMoves(prefix, solution){
  consoleOut += `${prefix}: `;
  const moves = solution[prefix];
  for (let i = 0; i < moves.length; i++){
    consoleOut += `(${moves[i].X},${moves[i].Y})`
  }
  consoleOut += '\n';
}

function drawLinesCar(solutions, car, getMoves){
  for (let i = 0; i < solutions.length; i++){
    ctx.strokeStyle = (i < solutions.length - 1) ? 'rgba(255,0,0,.1)' : 'rgb(255,255,0)';
    let pos = car.Pos;
    let v = car.V;
    const moves = getMoves(solutions[i]);
    const points = [pos];

    for (let j = 0; j < moves.length; j++){
      v = {X: v.X + moves[j].X, Y: v.Y + moves[j].Y}
      pos = {X: pos.X + v.X, Y: pos.Y + v.Y}

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
  ctx.fill(createDisk(o.Pos.X, o.Pos.Y, o.Radius));
}

function drawFlag(flag, index, isNext) {
  ctx.fillStyle = isNext ? 'cyan' : 'blue';
  ctx.fill(createDisk(flag.X, flag.Y));
}

function drawCar(state, prefix) {
  const car = state[prefix];
  ctx.fillStyle = car.IsAlive ? 'green' : 'red';
  ctx.fill(createDisk(car.Pos.X, car.Pos.Y, car.Radius));
  consoleOut += `${prefix}: ${JSON.stringify(car)}\n`;
}

function createLine(points){
  const res = new Path2D();
  if (points.length > 0) {
    const start = points[0];
    res.moveTo(toScreenX(start.X), toScreenY(start.Y));
    for (let p of points)
      res.lineTo(toScreenX(p.X), toScreenY(p.Y));
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
  const screenX = toScreenX(pos.X);
  const screenY = toScreenY(pos.Y);
  return screenX > 0 && screenX < canvas.width && screenY > 0 && screenY < canvas.height;
}

function adjustScale(state) {
  let objects = [
      ...race.Track.Flags,
      ...race.Track.Obstacles.map(o => o.Pos),
         state.FirstCar.Pos,
         state.SecondCar.Pos
  ];
  cellSize = 8;
  while (cellSize > 0.2 && objects.some(p => !fitInScreen(p)))
    cellSize/=1.2;
}
