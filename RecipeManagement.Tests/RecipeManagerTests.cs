using System.Collections.Generic;
using RecipeManagement.Core;

namespace RecipeManagement.Tests;

/// <summary>
/// Example tests from the assignment specification. Add your own tests as you work.
/// </summary>
public sealed class RecipeManagerTests
{
    [Fact]
    public void Constructor_BuildsRecipeDictionary()
    {
        var manager = CreateManager();
        Assert.Equal(2, manager.RecipeCount);
        Assert.Equal("Recipe A", manager.FindRecipe(10)?.Title);
    }

    [Fact]
    public void InstructionsAreCompletedInFileOrder()
    {
        var manager = CreateManager();
        Assert.True(manager.StartCooking(10));
        Assert.Equal("First step", manager.PeekNextInstruction());
        Assert.Equal("First step", manager.CompleteNextInstruction());
        Assert.Equal("Second step", manager.PeekNextInstruction());
    }

    [Fact]
    public void RemovedRecipesAreRestoredLastInFirstOut()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);
        manager.RemoveRecipeFromCookingPlan(20);
        Assert.Equal(20, manager.PeekLastRemovedRecipe());
        Assert.True(manager.RestoreLastRemovedRecipe());
        Assert.Equal(new[] { 20 }, manager.GetCookingPlan());
    }

    private static RecipeManager CreateManager()
    {
        return new RecipeManager(new[]
        {
            new Recipe
            {
                Id = 10,
                Title = "Recipe A",
                Ingredients = new() { "1 apple" },
                Instructions = new() { "First step", "Second step" }
            },
            new Recipe
            {
                Id = 20,
                Title = "Recipe B"
            }
        });
    }

    [Fact]
    public void AddRecipe_ReturnsTrue_WhenRecipeAddSuccessful()
    {
        var manager = CreateManager();
        Recipe testRecipe = new()
        {
            Id = 30,
            Title = "Recipe C",
            Ingredients = ["5 cups of Sugar"],
            Instructions = ["First step", "Second step"]
        };
        
        bool result = manager.AddRecipe(testRecipe);

        Assert.True(result);
        Assert.Contains(30, manager.RecipeDatabase.Keys);
    }

    [Fact]
    public void AddRecipe_ReturnsFalse_WhenNegativeRecipeAddFails()
    {
        var manager = CreateManager();
        Recipe testRecipe = new()
        {
            Id = -10,
            Title = "Recipe C",
            Ingredients = ["5 cups of Sugar"],
            Instructions = ["First step", "Second step"]
        };
        
        bool result = manager.AddRecipe(testRecipe);

        Assert.False(result);
        Assert.DoesNotContain(-10, manager.RecipeDatabase.Keys);
    }

    [Fact]
    public void AddRecipe_ReturnsFalse_WhenDuplicateRecipeAddFails()
    {
        var manager = CreateManager();
        Recipe testRecipe = new()
        {
                Id = 10,
                Title = "Recipe A",
                Ingredients = ["1 apple"],
                Instructions = ["First step", "Second step"]
        };
        
        bool result = manager.AddRecipe(testRecipe);

        Assert.False(result);
        Assert.Contains(10, manager.RecipeDatabase.Keys);
    }


    [Fact]
    public void FindRecipe_ReturnsRecipe_WhenRecipeFound()
    {
        var manager = CreateManager();

        var result = manager.FindRecipe(10);

        Assert.NotNull(result);
        Assert.IsType<Recipe>(result);
        Assert.Equal(10, result.Id);
        Assert.Contains(result, manager.RecipeDatabase.Values);
    }

    [Fact]
    public void FindRecipe_ReturnsNull_WhenRecipeNotFound()
    {
        var manager = CreateManager();

        var result = manager.FindRecipe(30);

        Assert.Null(result);
    }

    [Fact]
    public void RemoveRecipe_ReturnsTrue_WhenRecipeRemoved()
    {
        var manager = CreateManager();

        var result = manager.RemoveRecipe(20);

        Assert.True(result);
        Assert.Null(manager.FindRecipe(20));
    }

    [Fact]
    public void RemoveRecipe_ReturnsFalse_WhenInvalidRecipe()
    {
        var manager = CreateManager();

        var result = manager.RemoveRecipe(30);

        Assert.False(result);
        Assert.Null(manager.FindRecipe(30));
    }

    [Fact]
    public void AddIngredientsToShoppingList_ReturnsInt_WhenRecipeFound()
    {
        var manager = CreateManager();

        var result = manager.AddIngredientsToShoppingList(10);

        Assert.NotEmpty(manager.ShoppingList);
        Assert.Equal(1, result);
    }

    [Fact]
    public void AddIngredientsToShoppingList_ReturnsNull_WhenInvalidRecipe()
    {
        var manager = CreateManager();

        var result = manager.AddIngredientsToShoppingList(30);

        Assert.Empty(manager.ShoppingList);
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetShoppingList_ReturnsList()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);
        manager.AddIngredientsToShoppingList(20);

        var result = manager.GetShoppingList();

        Assert.NotEmpty(manager.ShoppingList);
        Assert.IsType<List<string>>(result);
        Assert.Single(result);
        Assert.Equal("1 apple", result[0]);
    }

    [Fact]
    public void ClearShoppingList_ClearShoppingList()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);
        manager.AddIngredientsToShoppingList(20);

        manager.ClearShoppingList();

        Assert.Empty(manager.ShoppingList);
    }

    [Fact]
    public void AddRecipeToCookingPlan_ReturnTrue_WhenValidRecipe()
    {
        var manager = CreateManager();

        bool result = manager.AddRecipeToCookingPlan(10);

        Assert.True(result);
        Assert.Contains(10, manager.CookingPlan);
    }
    [Fact]
    public void AddRecipeToCookingPlan_ReturnFalse_WhenInvalidRecipe()
    {
        var manager = CreateManager();

        bool result = manager.AddRecipeToCookingPlan(30);

        Assert.False(result);
        Assert.DoesNotContain(30, manager.CookingPlan);
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_ReturnTrue_WhenValidRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);

        bool result = manager.RemoveRecipeFromCookingPlan(10);

        Assert.True(result);
        Assert.DoesNotContain(10, manager.CookingPlan);
        Assert.Contains(20, manager.CookingPlan);
        Assert.Contains(10, manager.RemovedRecipeHistory);
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_ReturnFalse_WhenInvalidRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);

        bool result = manager.RemoveRecipeFromCookingPlan(30);

        Assert.False(result);
        Assert.Contains(20, manager.CookingPlan);
        Assert.DoesNotContain(30, manager.CookingPlan);
        Assert.DoesNotContain(30, manager.RemovedRecipeHistory);
    }
    
    //TODO
    [Fact]
    public void RestoreLastRemovedRecipe_ReturnTrue_WhenValidRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);

        bool result = manager.RemoveRecipeFromCookingPlan(30);

        Assert.False(result);
        Assert.Contains(20, manager.CookingPlan);
        Assert.DoesNotContain(30, manager.CookingPlan);
        Assert.DoesNotContain(30, manager.RemovedRecipeHistory);
    }
}
