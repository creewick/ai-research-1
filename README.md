# Исследование

## Цель исследования
На примере конкреткой задачи научиться применять наборы алгоритмов.

Научиться искать объективную выборку тестов и сравнивать поведения алгоритмов на этой выборке.

## Задача

Пример трассы | Описание
--------------|-------
![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/example.png) | Есть трасса с двумя машинками, наборами флагов и препятствий. <br> Есть строгий порядок сбора флагов, но не имеет значения, какая из машинок возьмет флаг.  <br><br> Каждый ход машинка может изменить своё ускорение на (x,y) -1 ≤ x,y ≤ 1 или вызвать команду Exchange. <br> Если обе машинки на текущем ходу вызвали команду Exchange, они обмениваются своими векторами скоростей. <br><br> На каждый ход машинкам даётся 100ms. <br> У команды Exchange есть cooldown – 20 ходов. <br><br>_Пояснения: <br> Синие – флаги; Голубой – след. флаг; Зеленые – машинки; Серые – преграды_

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
  <summary><b>Группы карт</b></summary>

Группа Без препятствий | Группа Мало препятствий
-----------------------|------------------------
![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/NoBlocks.png)|![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Blocks.png)
</details>
<details>
  <summary><b>Отдельные карты</b></summary>

Bottle Neck 1 | Bottle Neck 2 | Bottle Neck 3 | Exchange
:------------:|:-------------:|:-------------:|:--------:
![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/BottleNeck.png) | ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/BottleNeck2.png) | ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/BottleNeck3.png) | ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Exchange.png)
**Cross** | **10_10_3** | **5_10** | **7_10**
![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Cross.png) | ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/10_10_3.png) | ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/5_10.png) | ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/7_10.png)
**Sprint 1** | **Sprint 22** | **Snake**
![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Sprint.png) | ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Sprint2.png) | ![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Snake.png)
</details>

## Оптимальные параметры

Запуская алгоритмы на выбранных картах, мы искали оптимальные параметры для них. Остановились на следующих:

Алгоритмы             | Greedy      | Random      | Hill Climb   | Evolution
----------------------|-------------|-------------|--------------|---------------
**Параметры**         | Глубина: 15<br>Стратегия: `Repeat`<br>Оценка: `Max` | Глубина: 11<br>Макс.сегмент: 9 | BaseSolver: `Greedy`<br>Эвристика: `true`<br>BaseSolverTime: 1/10 | BaseSolver: `Greedy`<br>Filter: `FilterHalf`<br>Applier: `SegmentCrossingOver`<br>Selector: `Elitism`

## Результаты

[Промежуточные вычисления](https://docs.google.com/spreadsheets/d/1jnzvyOMs1Fs-sn62Y32mR_D2tlMVwEJQ1C1jdlvhAr8/edit?usp=sharing)

Алгоритмы                    |  | Greedy       |  | Random        |  | Hill Climbing  |  | Evolution
-----------------------------|--|--------------|--|---------------|--|----------------|--|---------------
**Группа Без препятствий**   |🥇| 792 ± 18.2  |🥈| 745 ± 17.6    |🥉| 647 ± 15.2    |🥈| 725 ± 21.5  
**Группа Мало препятствий**  |🥇| 714 ± 34.2  |👎| 548 ± 39.6    |🥉| 596 ± 29.3    |🥈| 655 ± 31.7
**Exchange**                 |🥇| 182 ± 13.5  |👎| 138 ± 17.6    |🥉| 167 ± 13.9    |🥈| 171 ± 14.4
**Bottle Neck 1**            |🥇| 312.4 ± 15.2|🥉| 195.1 ± 35.7  |🥈|288.5 ± 21     |👎| 179.2 ± 36.7
**Bottle Neck 2**            |🥇| 214.3 ± 10.5|👎| 18.14 ± 58.4  |🥈| 174.4 ± 41.5  |🥉| 73.1 ± 48.6
**Bottle Neck 3**            |👎| -380 ± 18.5 |🥉| -192.8 ± 64.7 |🥇| 239.9 ± 17.2  |🥈| -93.3 ± 55.2
**Sprint 1**                 |👎|-427 ± 20.8  |🥈| 145.4 ± 24.1  |🥇| 180.6 ± 11    |🥉| -88 ± 34.3
**Sprint 2**                 |👎|-240 ± 11.7  |🥈| 152.8 ± 19.7  |🥇| 160.4 ± 21    |🥉| -58.3 ± 24.6
**Cross**                    |🥇| 95.2 ± 4.7  |🥈|51 ± 5.9       |🥉| 43.6 ± 4.6    |👎| 35 ± 5.1
**10_10_3**                  |🥈| 87.6 ± 4.3  |🥇|105.5 ± 7.5    |👎| 54.8 ± 6.8    |🥉| 84.3 ± 6.9
**5_10**                     |🥈| 228 ± 7.3   |🥇| 236 ± 2       |🥉| 202 ± 10.8    |🥉| 201 ± 13
**7_10**                     |🥇| 41 ± 2      |🥇| 40.1 ± 3      |👎| 27.4 ± 2.5    |🥇| 38.6 ± 2.7
**Snake**                    |🥇| 171.4 ± 8.3 |🥈| 121.5 ± 8.9   |👎| 78.6 ± 17.6   |🥉| 85.7 ± 21.5

### Выводы
> Из-за проблем с тестами не хватило времени, чтобы прогнать тесты достаточное кол-во раз (чтобы интервалы везде не пересекались)

> `Greedy` работает лучше всех на тестах без препятствий и там, где нужно сильно маневрировать

> `Hill Climbing` работает лучше всех на тестах с маленькими коридорами (`Bottle Neck`, `Sprint`)

> `Evolution` хорошо работает на карте Exchange

> `Random` хорошо работает на картах с большим количеством препятствий

## Реплеи

<details>
  <summary><b>Greedy – 10_10_3</b></summary>

![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Greedy_10_10_3.gif)
</details><details>
  <summary><b>Hill Climbing – 7_10</b></summary>

![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Hill_7_10.gif)
</details>

### Ломающие тесты

Некоторые тесты мы внедрили после подбора параметров, и подобрали их так, чтобы они ломали некоторые алгоритмы.

Вот некоторые реплеи с этими тестами:

<details>
  <summary><b>Greedy – Sprint</b></summary>

![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Greedy_Sprint.gif)
</details><details>
  <summary><b>Random – Bottle Neck 2</b></summary>

![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Random_BottleNeck2.gif)
</details><details>
  <summary><b>Evolution – Cross</b></summary>

![Картинка](https://github.com/creewick/ai-research-1/blob/master/Images/Evolution_Cross.gif)
</details>

