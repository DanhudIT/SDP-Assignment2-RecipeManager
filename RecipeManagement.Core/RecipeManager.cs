using System;
using System.Collections.Generic;

namespace RecipeManagement.Core;

/// <summary>
/// Implement this class using the five Part A collections as private fields:
/// Dictionary&lt;int, Recipe&gt;, List&lt;string&gt;, LinkedList&lt;int&gt;,
/// Stack&lt;int&gt; and Queue&lt;string&gt;.
/// </summary>
public sealed class RecipeManager : IRecipeManager
{
    // Private Collection fields
    private Dictionary<int, Recipe> recipeDatabase = new();
    private List<string> shoppingList = new();
    private LinkedList<int> cookingPlan = new();
    private Stack<int> removedRecipeHistory = new();
    private Queue<string> instructionQueue = new();

    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        //To ensure the user doesn't submit a null recipe collection
        ArgumentNullException.ThrowIfNull(recipes);

        foreach(Recipe recipeImport in recipes)
        {
            // to validate the recipe for negatives and duplicates; invalid recipes are rejected and thrown.
            ArgumentNullException.ThrowIfNull(recipeImport);

            if(recipeImport.Id < 0)
            {
                throw new ArgumentException($"Negative recipe ID: Negative Recipe ID cannot be negative!");
            }
            if(string.IsNullOrWhiteSpace(recipeImport.Title))
            {
                throw new ArgumentException($"Blank Title: Recipe has no title, unable to be imported!");
            }

            bool validRecipe = recipeDatabase.TryAdd(recipeImport.Id, recipeImport);
            if(!validRecipe)
            {
                throw new ArgumentException($"Duplicate Recipe ID: {recipeImport.Id} already exists, unable to be imported!");
            }
        }
    }

    public int RecipeCount => recipeDatabase.Count;
    public int ShoppingItemCount => shoppingList.Count;
    public int CookingPlanCount => cookingPlan.Count;
    public int PendingInstructionCount => instructionQueue.Count;
    public int RemovedRecipeCount => removedRecipeHistory.Count;

    public bool AddRecipe(Recipe recipe)
    {
        // Error checking is required for the ID, Title and duplicates; invalid recipes are rejected and returned as false.
        ArgumentNullException.ThrowIfNull(recipe);

        if(recipe.Id <= 0)
        {
            return false;
        }
        if(string.IsNullOrWhiteSpace(recipe.Title))
        {
            return false;
        }

        return recipeDatabase.TryAdd(recipe.Id, recipe);
    }

    public Recipe? FindRecipe(int recipeId)
    {
        recipeDatabase.TryGetValue(recipeId, out Recipe? validRecipe);
        return validRecipe;
    }

    public bool RemoveRecipe(int recipeId)
    {
        bool taskCompletion = false;
        if(!cookingPlan.Contains(recipeId))
        {
            taskCompletion = recipeDatabase.Remove(recipeId);
        }
        return taskCompletion;
    }

    public int AddIngredientsToShoppingList(int recipeId)
    {
        int ingredientCount = 0;
        Recipe? indexedRecipe = FindRecipe(recipeId);
        if(indexedRecipe is null)
            return ingredientCount;
        foreach(string ingredient in indexedRecipe.Ingredients)
        {
            shoppingList.Add(ingredient);
            ingredientCount += 1;
        }
        return ingredientCount;
    }

    public IReadOnlyList<string> GetShoppingList()
    {
        return shoppingList.ToList();
    }

    public void ClearShoppingList()
    {
        shoppingList.Clear();
    }


    public bool AddRecipeToCookingPlan(int recipeId)
    {
        bool taskCompletion = false;
        if(FindRecipe(recipeId) is Recipe)
        {
            if(!cookingPlan.Contains(recipeId))
            {
                cookingPlan.AddLast(recipeId);
                taskCompletion = true;
            }
        }        
        return taskCompletion;
    }

    public bool RemoveRecipeFromCookingPlan(int recipeId)
    {
        bool taskCompletion = false;
        if(cookingPlan.Remove(recipeId))
        {
            removedRecipeHistory.Push(recipeId);
            taskCompletion = true;
        }
        return taskCompletion;
    }

    public bool RestoreLastRemovedRecipe()     
    {
        bool taskCompletion = false;
        if(removedRecipeHistory.TryPop(out int recipeId))
        {
            taskCompletion = AddRecipeToCookingPlan(recipeId);
        }
        return taskCompletion;
    }

    public int? PeekLastRemovedRecipe()
    {
        int? recipeId = removedRecipeHistory.TryPeek(out int result) ? result : null;
        return recipeId;
    }

    public IReadOnlyList<int> GetCookingPlan()
    {
        return cookingPlan.ToList();
    }

    public bool StartCooking(int recipeId)
    {
        bool taskCompletion = false;
        Recipe? newRecipe = FindRecipe(recipeId);
        if(newRecipe?.Instructions.Count > 0)
        {
            instructionQueue.Clear();
            foreach (string instruction in newRecipe.Instructions)
            {
                instructionQueue.Enqueue(instruction);
            }
            taskCompletion = true;            
        }
        return taskCompletion;
    }

    public string? PeekNextInstruction()
    {
        instructionQueue.TryPeek(out string? nextInstruction);
        return nextInstruction;
    }

    public string? CompleteNextInstruction()
    {
        instructionQueue.TryDequeue(out string? nextInstruction);
        return nextInstruction;
    }

    public IReadOnlyList<Recipe> SearchByTitle(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByTitle.");

    public IReadOnlyList<Recipe> SearchByIngredient(string searchText) =>
        throw new NotImplementedException("Part B: implement SearchByIngredient.");

    public IReadOnlyList<Recipe> GetHighestProteinRecipes(int count) =>
        throw new NotImplementedException("Part B: implement GetHighestProteinRecipes.");

    public bool AddSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement AddSavedRecipe.");

    public bool RemoveSavedRecipe(int recipeId) =>
        throw new NotImplementedException("Part B: implement RemoveSavedRecipe.");

    public bool IsRecipeSaved(int recipeId) =>
        throw new NotImplementedException("Part B: implement IsRecipeSaved.");

    public IReadOnlyList<int> GetSavedRecipes() =>
        throw new NotImplementedException("Part B: implement GetSavedRecipes.");
}