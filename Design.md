# Design

## 1. Saved recipe collection

`HashSet<int> savedRecipes`

A `HashSet<int>` suits all four requirements. `Add` returns false for a duplicate, so duplicate prevention is built in. `Add`, `Remove` and membership checks (`Contains`) have average O(1) complexity through hashing.

I first considered a `Dictionary<int, Recipe>` but its stored recipe references could go stale if the recipe catalogue replaced recipes with same ID. Storing only the ids as a `HashSet<int>` keeps `recipeDatabase` as the single source of truth.

## 2. Integration

The Part B features the recipes stored in the catalogue that Part A maintains and then filters them with the `SearchByTitle()`, `SearchByIngredient()` and `GetHighestProteinRecipes()` methods. The save feature keeps its own set of recipe ids, but the `FindRecipe()` method from Part A is used to check that a recipe exists before `AddSavedRecipe()` stores its id. The `GetHighestProteinRecipes()` searches through the `Recipe.Nutrition.ProteinG` and skips null `ProteinG`, orders them by highest and limits list size by count. Additionally, `RemoveRecipe()` was updated to automatically remove the corresponding id from the saved set, so a deleted recipe can't remain saved.


## 3. Basic complexity

| Operation | Structure | Expected complexity | Reason |
| --- | --- | --- | --- |
| Lookup recipe by ID | Dictionary | Average O(1) | Hash-based key lookup. |
| Traverse cooking plan | LinkedList | O(n) | Each planned recipe may need to be visited. |
| Complete next instruction | Queue | O(1) | The item at the front is removed. |
| LINQ title/ingredient search | Recipe collection | O(n) | Each recipe may need to be inspected. |
| Check whether a recipe is saved | HashSet | Average O(1) | Hash-based lookup of id in set. |
