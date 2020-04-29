# Исследование

## Цель исследования
На примере конкреткой задачи научиться применять наборы алгоритмов.

Научиться искать объективную выборку тестов и сравнивать поведения алгоритмов на этой выборке.

## Задача

Есть трасса с двумя машинками, наборами флагов и препятствий. 
Есть строгий порядок сбора флагов, но не имеет значения, какая из машинок возьмет флаг.

Каждый ход машинка может изменить своё ускорение на (x,y) -1 ≤ x,y ≤ 1 или вызвать команду Exchange. 
Если обе машинки на текущем ходу вызвали команду Exchange, они обмениваются своими векторами скоростей.

На каждый ход машинкам даётся 100ms.
У команды Exchange есть cooldown – 20 ходов.

## Выбранные алгоритмы

1. Жадный поиск
2. Случайный поиск
3. Поиск восхождением
4. Генетический алгоритм
5. Практика использования последнего лучшего решения

Все подходы используют эмуляцию на неколько шагов вперед и оценивают решение одной из пяти функций оценки, представленных в классе `Emulator`.

### Жадный поиск (`GreedySolver`)
Классический жадный поиск, перебирающий все возможные ходы, и повторяющий один и тот же ход N раз.

### Случайный поиск (`RandomSolver`)
Генерирует в течении всего допустимого времени пары `(command, repeat)`, из который составляются решения. 

При помощи функции оценки выбирается лучшее.

Имеет возможность запускаться с эвристикой _сохранения последнего лучшего решения_, изменяет его для поиска новых решений.

### Поиск восхождением (`HillClimbingSolver`)
Для получения первого решения использует `GreedySolver` или `RandomSolver`. Применяет несколько типов мутаций, а потом выбирает лучшую.

Мутации:
- **Мутация случайного сегмента**

  Принимает количество сегментов, на которые нужно разбить решение и количество мутируемых сегментов. Случайно выбирает несколько мутируемых сегментов и случайно меняет в них команды.
- **Мутация переворачивания случайного сегмента**

  Принимает количество сегментов, на которые нужно разбить решение и количество мутируемых сегментов. Выбранные случайно сегменты переворачиваются.
- **Мутация замены двух соседних сегментов**

  Два случайно выбранных соседних сегмента меняются местами.
   
Реализована техника использования последнего лучшего решения. Включается, если передать соответствующий флаг.

### Генетический алгоритм (`EvolutionSolver`)
Для получения первого решения (популяции) использует предыдущие алгоритмы или их комбинацию в различных пропорциях (класс `CombinedSolver`).
Для получения следующих решений, популяция проходит через несколько шагов:

1. **Выбираются предки, которые будут изменяться**

   За выбор предков ответственен `IGeneticFilter`. На текущий момент есть две реализации:
   * `HalfFilter` сортирует решения по очкам и выбирает половину лучших решений
   * В `NormalizeFilter` шанс выбора определенного решения равен нормализованному значению очков

2. **Выбранные предки преобразовываются в потомков**
   
   За это отвечает `IGeneticApplier`. Есть две реализации:
   * `MutationApplier` позволяет использовать любую мутацию, совместимую с `HillClimbingSolver`
   * `SegmentCrossingOver` рассматривает пары предков, разделяет их решения по случайному числу K (на первые K шагов и остальные), берет первую часть от первого предка, вторую - от второго

3. **Из предков и потомков выбирается новая популяция**
   За это отвечает `IGeneticSelector`. Есть две реализации:
   * `Elitism` оставляет одного лучшего предка и выбирает лучших потомков
   * `ElitismRandom` помимо этого добавляет еще одно случайное решение

## Система тестов

![Система тестов](/Images/maps.png)

## Результаты (beta)

Результаты            | Greedy      | Random      | Hill Climb   | Evolution
----------------------|-------------|-------------|--------------|---------------
**Параметры**         | Глубина: 20<br>Стратегия: Повтор<br>Оценка: Max | ? | ? | BaseSolver: `Greedy`<br>Filter: `FilterHalf`<br>Applier: `SegmentCrossingOver`<br>Selector: `Elitism`
**Score ± ConfInt/2** | 1666 ± 24.2 |   ?         | ?            | 1553 ± 39.3
