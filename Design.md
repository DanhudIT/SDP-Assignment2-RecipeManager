# Design

Complete this document for **Part B**.

## 1. Saved recipe collection

Name the C# collection you selected for the saved/favourite feature and explain why it suits add, remove, duplicate prevention and membership checks.

`HashSet<int> savedRecipes`

A `HashSet<int>` suits all four requirements. `Add` returns false for a duplicate, so duplicate prevention is built in. `Add`, `Remove` and membership checks (`Contains`) have average O(1) complexity through hashing.

I first considered a `Dictionary<int, Recipe>` but its stored recipe copies could go stale if the recipe catalogue changed. Storing only the ids as a `HashSet<int>` keeps `recipeDatabase` as the single source of truth.

## 2. Integration

In a short paragraph, explain how the new Part B features use the existing Part A `RecipeManager` and recipe catalogue.

The Part B features use the `recipeDatabase` dictionary for lookup with the `SearchByTitle()` and `SearchByIngredient()` methods. The save feature keeps its own set of recipe ids, but the `FindRecipe()` method from Part A is used to check that a recipe exists before `AddSavedRecipe()` stores its id. Additionally, `RemoveRecipe()` was updated to automatically remove the corresponding id from the saved set, so a deleted recipe can't remain saved.


## 3. Basic complexity

| Operation | Structure | Expected complexity | Reason |
| --- | --- | --- | --- |
| Lookup recipe by ID | Dictionary | Average O(1) | Hash-based key lookup. |
| Traverse cooking plan | LinkedList | O(n) | Each planned recipe may need to be visited. |
| Complete next instruction | Queue | O(1) | The item at the front is removed. |
| LINQ title/ingredient search | Recipe collection | O(n) | Each recipe may need to be inspected. |
| Check whether a recipe is saved | HashSet | Average O(1) | Hash-based lookup of id in set. |
