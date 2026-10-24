# Recipe Management System — Student Starter

Starter repository for Parts A and B. Implemented `RecipeManager` in Core; the Application menu and JSON loader were supplied.

## What is supplied

- `RecipeManagement.Core/Models/` — recipe model classes
- `RecipeManagement.Core/RecipeLoader.cs` — reads `data/recipes.json`
- `RecipeManagement.Core/IRecipeManager.cs` — public API
- `RecipeManagement.Application/` — console menu (options labelled PartA / PartB)
- `RecipeManagement.Tests/` — example tests
- `data/recipes.json` — recipe dataset

## What I implemented

**Part A** — `RecipeManager.cs` using:

- `Dictionary<int, Recipe> recipeDatabase` 
- `List<string> shoppingList`
- `LinkedList<int> cookingPlan`
- `Stack<int> removedRecipeHistory`
- `Queue<string> instructionQueue`

**Part B** — LINQ searches, protein report, saved-recipe collection, `Design.md`, and more tests.

- `HashSet<int> savedRecipes`

## Build and run

Open `RecipeManagement.sln`:

```bash
dotnet build
dotnet test
dotnet run --project RecipeManagement.Application -- data/recipes.json
```

## AI acknowledgement

AI acknowledgement: I used Claude Code to help me understand private fields and encapsulation, how to throw ArgumentExceptions, unit test construction methods, Assert methods, conversion of data types, troubleshooting setup and configuration issues with VS Code editor, review code against assignment requirements, to create to-do lists from assignment specs, understanding integration tests, stack and linked list behaviour and Claude was used for checklist creation of issues through Notion, LINQ and Lambda explanations.
I did not copy or adapt AI-generated code or other material into my submission. I developed the submitted solution myself based on my understanding of the course material.