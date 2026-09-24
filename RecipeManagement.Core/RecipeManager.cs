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
    // TODO Part A: add your private collection fields here.
    public Dictionary<int, Recipe> RecipeDatabase {get; private set; } = new();
    public List<string> ShoppingList {get; private set; } = new();
    public LinkedList<int> CookingPlan {get; private set; } = new();
    public Stack<int> RemovedRecipeHistory {get; private set; } = new();
    public Queue<string> InstructionQueue {get; private set; } = new();

    // TODO error checking.
    public RecipeManager(IEnumerable<Recipe> recipes)
    {
        // TODO Part A: validate recipes and build Dictionary<int, Recipe>.
        foreach(Recipe recipeImport in recipes)
        {
            // to validate the recipe for negatives and duplicates
            if (recipeImport.Id < 0)
            {
                Console.WriteLine($"Negative recipe ID: Negative Recipe ID cannot be negative!");
                continue;
            }
            bool validRecipe = RecipeDatabase.TryAdd(recipeImport.Id, recipeImport);
            if(!validRecipe)
            {
                Console.WriteLine($"Duplicate Recipe ID: {recipeImport.Id} already exists, unable to be imported!");
            }
        }
    }

    public int RecipeCount => RecipeDatabase.Count;
    public int ShoppingItemCount => ShoppingList.Count;
    public int CookingPlanCount => CookingPlan.Count;
    public int PendingInstructionCount => 0;
    public int RemovedRecipeCount => RemovedRecipeHistory.Count;

    // TODO error checking.
    public bool AddRecipe(Recipe recipe)
    {
        bool validRecipe = false;            
        if (recipe.Id < 0)
        {
            Console.WriteLine($"Negative recipe ID: Negative Recipe ID cannot be negative!");
            return validRecipe;
        }

        validRecipe = RecipeDatabase.TryAdd(recipe.Id, recipe);

        if(!validRecipe)
        {
            Console.WriteLine($"Duplicate Recipe ID: {recipe.Id} already exists, unable to be imported!");
            return validRecipe;
        }
        return validRecipe;
    }

    // TODO error checking.
    public Recipe? FindRecipe(int recipeId)
    {
        if(!RecipeDatabase.TryGetValue(recipeId, out Recipe? validRecipe))
            throw new ArgumentException($"Null:{recipeId} TODO error");
        return validRecipe;
    }

    // TODO error checking.
    public bool RemoveRecipe(int recipeId)
    {
        return RecipeDatabase.Remove(recipeId);
    }

    // TODO error checking.
    // Find recipe already has the recipe call and error handling, 
    public int AddIngredientsToShoppingList(int recipeId)
    {
        int ingredientCount = 0;
        Recipe? indexedRecipe = FindRecipe(recipeId);
        foreach(string ingredient in indexedRecipe?.Ingredients)
        {
            ShoppingList.Add(ingredient);
            ingredientCount += 1;
        }
        return ingredientCount;
    }

    // TODO error checking.
    public IReadOnlyList<string> GetShoppingList()
    {
        return ShoppingList;
    }

    public void ClearShoppingList()
    {
        ShoppingList.Clear();
    }


    // TODO error checking.
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
            else
            {
                throw new ArgumentException($"Duplicate Recipe ID: {recipeId} already exists in the cooking list!");
            }
        }        
        return taskCompletion;
    }

    // TODO
    public bool RemoveRecipeFromCookingPlan(int recipeId)
    {
        bool taskCompletion = false;
        if(CookingPlan.Remove(recipeId))
        {
            RemovedRecipeHistory.Push(recipeId);
            taskCompletion = true;
        }
        else
        {
            throw new ArgumentException($"Invalid Recipe ID: {recipeId} not in Cooking Plan!");
        }
        return taskCompletion;
    }

    public bool RestoreLastRemovedRecipe()     
    {
        bool taskCompletion = false;
        if(RemovedRecipeHistory.TryPop(out int recipeId))
        {
            AddRecipeToCookingPlan(recipeId);
            taskCompletion = true;
        }
        return taskCompletion;
    }
    public int? PeekLastRemovedRecipe()
    {
        RemovedRecipeHistory.TryPeek(out int recipeId);
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

        if(currentRecipe is not null)
        {
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
    // current interpretation: to remove the current instruction and output the result for interface to catch.
    // previous interpretation: remove current item in queue and print next item, if there is no next recipe then will call start cooking to attempt another recipe.
    {
        InstructionQueue.TryDequeue(out string? nextInstruction);
        // if(InstructionQueue.TryDequeue(out string? nextInstruction))
        // {
        //     nextInstruction = PeekNextInstruction();
        //     if(PeekNextInstruction() is null)
        //     {
        //         RemoveRecipeFromCookingPlan((int) CookingPlan.First.Value);
        //         StartCooking((int) CookingPlan.First.Value);
        //         nextInstruction = PeekNextInstruction();
        //     }
        // }
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

//TODO
// Dictionary<int, Recipe>
// List<string>
// LinkedList<int>
// Stack<int>
// Queue<string>