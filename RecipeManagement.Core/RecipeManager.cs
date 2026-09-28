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
    // Private Collections
    private Dictionary<int, Recipe> RecipeDatabase {get; set; } = new();
    private List<string> ShoppingList {get; set; } = new();
    private LinkedList<int> CookingPlan {get; set; } = new();
    private Stack<int> RemovedRecipeHistory {get; set; } = new();
    private Queue<string> InstructionQueue {get; set; } = new();

    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        foreach(Recipe recipeImport in recipes)
        {
            // to validate the recipe for negatives and duplicates; invalid recipes are rejected and thrown.
            ArgumentNullException.ThrowIfNull(recipeImport);

            if(recipeImport.Id <= 0)
            {
                throw new ArgumentException($"Negative recipe ID: Negative Recipe ID cannot be negative!");
            }
            if(string.IsNullOrWhiteSpace(recipeImport.Title))
            {
                throw new ArgumentException($"Blank Title: Recipe has no title, unable to be imported!");
            }

            bool validRecipe = RecipeDatabase.TryAdd(recipeImport.Id, recipeImport);
            if(!validRecipe)
            {
                throw new ArgumentException($"Duplicate Recipe ID: {recipeImport.Id} already exists, unable to be imported!");
            }
        }
    }

    public int RecipeCount => RecipeDatabase.Count;
    public int ShoppingItemCount => ShoppingList.Count;
    public int CookingPlanCount => CookingPlan.Count;
    public int PendingInstructionCount => InstructionQueue.Count;
    public int RemovedRecipeCount => RemovedRecipeHistory.Count;

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

        return RecipeDatabase.TryAdd(recipe.Id, recipe);
    }

    public Recipe? FindRecipe(int recipeId)
    {
        RecipeDatabase.TryGetValue(recipeId, out Recipe? validRecipe);
        return validRecipe;
    }

    public bool RemoveRecipe(int recipeId)
    {
        bool taskCompletion = false;
        if(!CookingPlan.Contains(recipeId))
        {
            taskCompletion = RecipeDatabase.Remove(recipeId);
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
            ShoppingList.Add(ingredient);
            ingredientCount += 1;
        }
        return ingredientCount;
    }

    public IReadOnlyList<string> GetShoppingList()
    {
        return ShoppingList.ToList();
    }

    public void ClearShoppingList()
    {
        ShoppingList.Clear();
    }


    public bool AddRecipeToCookingPlan(int recipeId)
    {
        bool taskCompletion = false;
        if(FindRecipe(recipeId) is Recipe)
        {
            if(!CookingPlan.Contains(recipeId))
            {
                CookingPlan.AddLast(recipeId);
                taskCompletion = true;
            }
        }        
        return taskCompletion;
    }

    public bool RemoveRecipeFromCookingPlan(int recipeId)
    {
        bool taskCompletion = false;
        if(CookingPlan.Remove(recipeId))
        {
            RemovedRecipeHistory.Push(recipeId);
            taskCompletion = true;
        }
        return taskCompletion;
    }

    public bool RestoreLastRemovedRecipe()     
    {
        bool taskCompletion = false;
        if(RemovedRecipeHistory.TryPop(out int recipeId))
        {
            if(AddRecipeToCookingPlan(recipeId))
            {
                taskCompletion = true;
            }
            
        }
        return taskCompletion;
    }

    public int? PeekLastRemovedRecipe()
    {
        int? recipeId = RemovedRecipeHistory.TryPeek(out int result) ? result : null;
        return recipeId;
    }

    public IReadOnlyList<int> GetCookingPlan()
    {
        return CookingPlan.ToList();
    }

    public bool StartCooking(int recipeId)
    {
        bool taskCompletion = false;
        Recipe? currentRecipe = FindRecipe(recipeId);
        if(currentRecipe?.Instructions is not null)
        {
            InstructionQueue.Clear();
            foreach (string instruction in currentRecipe.Instructions)
            {
                InstructionQueue.Enqueue(instruction);
            }
            taskCompletion = true;            
        }

        return taskCompletion;
    }

    public string? PeekNextInstruction()
    {
        InstructionQueue.TryPeek(out string? nextInstruction);
        return nextInstruction;
    }

    public string? CompleteNextInstruction()
    {
        InstructionQueue.TryDequeue(out string? nextInstruction);
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