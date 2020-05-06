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
Все подходы используют эмуляцию на несколько шагов вперед и оценивают решение одной из функций оценок, представленных в классе [`Emulator`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Emulator.cs). В частности, по результатам тестрирования была выбрана функция [`GetScore_3`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Emulator.cs#L105).

<details>
  <summary><b>Жадный поиск</b></summary>
  
  [`GreedySolver`](https://github.com/creewick/ai-research-1/blob/master/Solvers/GreedySolver.cs)

  Классический жадный поиск, перебирающий все возможные ходы, и повторяющий один и тот же ход N раз.
</details>
<details>
  <summary><b>Случайный поиск</b></summary>
  
  [`RandomSolver`](https://github.com/creewick/ai-research-1/blob/master/Solvers/RandomSolver.cs)

  Алгоритм в течении всего допустимого времени на ход генерирует пары `(command, repeat)`, из которых составляются решения. При помощи функции оценки выбирается лучшее.
  
  Имеет возможность запускаться с эвристикой _сохранения последнего лучшего решения_, изменяет его для поиска новых решений.
</details>
<details>
  <summary><b>Поиск восхождением</b></summary>
  
  [`HillClimbingSolver`](https://github.com/creewick/ai-research-1/blob/master/Solvers/HillClimbing/HillClimbingSolver.cs)

  Поиск восхождением с использованием запоминанием последнего лучшего решения. Для получения первого решения использует `GreedySolver` или `RandomSolver`. Применяет несколько типов мутаций по принципу квот. Для распределения квот все мутаторы использовались одновременно и считалось, в какой доле случаев тот или иной мутатор выигрывал.

  #### Мутации:
  - Мутация случайного сегмента. Случайно выбирает количество сегментов, на которые нужно разбить решение и количество мутируемых сегментов. Случайно выбирает несколько мутируемых сегментов и случайно меняет в них команды одним из следующих способов:
    1) [Заполнение с повторением](https://github.com/creewick/ai-research-1/blob/master/Solvers/HillClimbing/Mutators/RandomRepeatSegmentMutator.cs)
    2) [Заполнение шумом](https://github.com/creewick/ai-research-1/blob/master/Solvers/HillClimbing/Mutators/RandomNoiseSegmentMutator.cs)
    3) [Заполнение бездействием](https://github.com/creewick/ai-research-1/blob/master/Solvers/HillClimbing/Mutators/RandomAndDoNothingSegmentMutator.cs)
  Статистика показала, что каждая из приведенных выше мутаций дает улучшение в 1/3 случаев.
  - Мутация переворачивания случайного сегмента. Принимает количество сегментов, на которые нужно разбить решение и количество мутируемых сегментов. Выбранные случайно сегменты переворачиваются.
  - Мутация замены двух соседних сегментов. Два случайно выбранных соседних сегмента меняются местами.
  Реализована техника использования последнего лучшего решения. Включается, если передать соответствующий флаг.
</details>
<details>
  <summary><b>Генетический алгоритм</b></summary>
  
  [`EvolutionSolver`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/EvolutionSolver.cs)
  
  Для получения первого решения (популяции) использует предыдущие алгоритмы или их комбинацию в различных пропорциях — [`CombinedSolver`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/BaseSolvers/CombinedSolver.cs).
  Для получения следующих решений, популяция проходит через несколько шагов:

  1. **Выбираются предки, которые будут изменяться**
     За выбор предков ответственен [`IGeneticFilter`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Filters/IGeneticFilter.cs). На текущий момент есть две реализации:
     * [`HalfFilter`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Filters/FilterHalf.cs) сортирует решения по очкам и выбирает половину лучших решений
     * В [`NormalizeFilter`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Filters/NormalizeFilter.cs) шанс выбора определенного решения равен нормализованному значению очков

  2. **Выбранные предки преобразовываются в потомков**
     За это отвечает [`IGeneticApplier`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Appliers/IGeneticApplier.cs). Есть две реализации:
     * [`MutationApplier`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Appliers/MutationApplier.cs) позволяет использовать любую мутацию, совместимую с `HillClimbingSolver`
     * [`SegmentCrossingOver`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Appliers/SegmentCrossingOver.cs) рассматривает пары предков, разделяет их решения по случайному числу K (на первые K шагов и остальные), берет первую часть от первого предка, вторую - от второго

  3. **Из предков и потомков выбирается новая популяция**
     За это отвечает [`IGeneticSelector`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Selectors/IGeneticSelector.cs). Есть две реализации:
     * [`Elitism`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Selectors/Elitism.cs) оставляет одного лучшего предка и выбирает лучших потомков
     * [`ElitismRandom`](https://github.com/creewick/ai-research-1/blob/master/Solvers/Evolution/Selectors/ElitismRandom.cs) помимо этого добавляет еще одно случайное решение
</details>

## Система тестов

Написать объективную выборку тестов, которая бы покрывала все реалии этой задачи, оказалось очень сложно.

Спустя пару недель мы сошлись на решении, в котором у нас есть несколько групп карт, и несколько отдельных обособленных случаев.

<details>
  <summary><b>Группа Без препятствий</b></summary>
  
  ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/NoBlocks.png)
</details>
<details>
  <summary><b>Группа Мало препятствий</b></summary>
  
  ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Blocks.png)
</details>
...

## Оптимальные параметры

Запуская алгоритмы на собранных картах, мы искали оптимальные параметры для них. Остановились на следующих:

Табличка              | Greedy      | Random      | Hill Climb   | Evolution
----------------------|-------------|-------------|--------------|---------------
**Параметры**         | Глубина: 15<br>Стратегия: `Repeat`<br>Оценка: `Max` | Глубина: 11<br>Макс.сегмент: 9 | BaseSolver: `Greedy(15)`<br>Эвристика: `true`<br>BaseSolverTime: 1/10 | BaseSolver: `Greedy(15)`<br>Filter: `FilterHalf`<br>Applier: `SegmentCrossingOver`<br>Selector: `Elitism`

## Результаты

[Промежуточные вычисления](https://docs.google.com/spreadsheets/d/1jnzvyOMs1Fs-sn62Y32mR_D2tlMVwEJQ1C1jdlvhAr8/edit?usp=sharing)

Результаты                   | Greedy      | Random      | Hill Climb   | Evolution
-----------------------------|-------------|-------------|--------------|---------------
**Группа без препятствий**   | | | |
...
