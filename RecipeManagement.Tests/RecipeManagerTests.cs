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
        Assert.NotNull(manager.FindRecipe(testRecipe.Id));
        Assert.Equal(3, manager.RecipeCount);
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
        Assert.Null(manager.FindRecipe(testRecipe.Id));
        Assert.Equal(2, manager.RecipeCount);
    }

    [Fact]
    public void AddRecipe_ReturnsFalse_WhenDuplicateRecipeAddFails()
    {
        var manager = CreateManager();
        Recipe testRecipe = new()
        {
                Id = 30,
                Title = "Recipe C",
                Ingredients = ["5 apple"],
                Instructions = ["First step", "Second step"]
        };
        
        bool result = manager.AddRecipe(testRecipe);
        bool resultFail = manager.AddRecipe(testRecipe);

        Assert.True(result);
        Assert.False(resultFail);
        var verifyRecipe = manager.FindRecipe(30);
        Assert.NotNull(verifyRecipe);
        Assert.Equal(30, verifyRecipe.Id);
    }


    [Fact]
    public void FindRecipe_ReturnsRecipe_WhenRecipeFound()
    {
        var manager = CreateManager();

        var result = manager.FindRecipe(10);

        Assert.NotNull(result);
        Assert.IsType<Recipe>(result);
        Assert.Equal(10, result.Id);
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

        Assert.NotEmpty(manager.GetShoppingList());
        Assert.Equal(1, manager.ShoppingItemCount);
        Assert.Equal(1, result);
    }

    [Fact]
    public void AddIngredientsToShoppingList_ReturnsZero_WhenInvalidRecipe()
    {
        var manager = CreateManager();

        var result = manager.AddIngredientsToShoppingList(30);

        Assert.Empty(manager.GetShoppingList());
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetShoppingList_ReturnsList()
    {
        var manager = CreateManager();
        manager.AddIngredientsToShoppingList(10);
        manager.AddIngredientsToShoppingList(20);

        var result = manager.GetShoppingList();

        Assert.NotEmpty(manager.GetShoppingList());
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

        Assert.Empty(manager.GetShoppingList());
    }

    [Fact]
    public void AddRecipeToCookingPlan_ReturnTrue_WhenValidRecipe()
    {
        var manager = CreateManager();

        bool result = manager.AddRecipeToCookingPlan(10);

        Assert.True(result);
        Assert.Contains(10, manager.GetCookingPlan());
        Assert.Equal(1, manager.CookingPlanCount);
    }
    [Fact]
    public void AddRecipeToCookingPlan_ReturnFalse_WhenInvalidRecipe()
    {
        var manager = CreateManager();

        bool result = manager.AddRecipeToCookingPlan(30);

        Assert.False(result);
        Assert.DoesNotContain(30, manager.GetCookingPlan());
        Assert.Equal(0, manager.CookingPlanCount);
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_ReturnTrue_WhenValidRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);

        bool result = manager.RemoveRecipeFromCookingPlan(10);

        Assert.True(result);
        Assert.DoesNotContain(10, manager.GetCookingPlan());
        Assert.Contains(20, manager.GetCookingPlan());
        Assert.Equal(1, manager.CookingPlanCount);
        Assert.Equal(10, manager.PeekLastRemovedRecipe());
    }

    [Fact]
    public void RemoveRecipeFromCookingPlan_ReturnFalse_WhenInvalidRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);

        bool result = manager.RemoveRecipeFromCookingPlan(30);

        Assert.False(result);
        Assert.Contains(20, manager.GetCookingPlan());
        Assert.DoesNotContain(30, manager.GetCookingPlan());
        Assert.Null(manager.PeekLastRemovedRecipe());
    }
    
    [Fact]
    public void RestoreLastRemovedRecipe_ReturnTrue_WhenValidRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);

        bool result = manager.RestoreLastRemovedRecipe();

        Assert.True(result);
        Assert.Contains(10, manager.GetCookingPlan());
        Assert.Null(manager.PeekLastRemovedRecipe());
    }

    [Fact]
    public void RestoreLastRemovedRecipe_ReturnFalse_WhenNothingInStack()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);
        manager.RestoreLastRemovedRecipe();

        bool result = manager.RestoreLastRemovedRecipe();

        Assert.False(result);
        Assert.Contains(10, manager.GetCookingPlan());
        Assert.Null(manager.PeekLastRemovedRecipe());
    }

    [Fact]
    public void PeekLastRemovedRecipe_ReturnInt_WhenStackHasRemovedRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);

        int? result = manager.PeekLastRemovedRecipe();

        Assert.Equal(10, result);
        Assert.DoesNotContain(10, manager.GetCookingPlan());
        Assert.Equal(1, manager.RemovedRecipeCount);

    }

    [Fact]
    public void PeekLastRemovedRecipe_ReturnNull_WhenStackHasNoRemovedRecipe()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);
        manager.RemoveRecipeFromCookingPlan(10);
        manager.RestoreLastRemovedRecipe();

        int? result = manager.PeekLastRemovedRecipe();

        Assert.Null(result);
        Assert.Contains(10, manager.GetCookingPlan());
    }

    [Fact]
    public void GetCookingPlan_ReturnList()
    {
        var manager = CreateManager();
        manager.AddRecipeToCookingPlan(10);
        manager.AddRecipeToCookingPlan(20);

        var result = manager.GetCookingPlan();

        Assert.Contains(10, result);
        Assert.Contains(20, result);
        Assert.Equal(2, result.Count);
        Assert.Equal(2, manager.CookingPlanCount);
    }

    [Fact]
    public void StartCooking_ReturnTrue_WhenValidRecipeId()
    {
        var manager = CreateManager();
        
        bool result = manager.StartCooking(10);
        string? instruction1 = manager.CompleteNextInstruction();
        string? instruction2 = manager.CompleteNextInstruction();
        string? instruction3 = manager.CompleteNextInstruction();

        Assert.True(result);
        Assert.NotNull(instruction1);
        Assert.Equal("First step", instruction1);
        Assert.Equal("Second step", instruction2);
        Assert.Null(instruction3);
        Assert.Null(manager.PeekNextInstruction());
    }

    [Fact]
    public void StartCooking_ReturnFalse_WhenInvalidRecipeId()
    {
        var manager = CreateManager();
        
        bool result = manager.StartCooking(30);

        Assert.False(result);
        Assert.Null(manager.PeekNextInstruction());
    }

    [Fact]
    public void PeekNextInstruction_ReturnString_WhenNextInstructionAvailable()
    {
        var manager = CreateManager();
        manager.StartCooking(10);

        string? result = manager.PeekNextInstruction();

        Assert.Equal("First step", result);
    }

    [Fact]
    public void PeekNextInstruction_ReturnNull_WhenNextInstructionUnAvailable()
    {
        var manager = CreateManager();
        manager.StartCooking(10);
        manager.CompleteNextInstruction();
        manager.CompleteNextInstruction();

        string? result = manager.PeekNextInstruction();

        Assert.Null(result);
    }


    [Fact]
    public void CompleteNextInstruction_ReturnString_WhenNextInstructionAvailable()
    {
        var manager = CreateManager();
        manager.StartCooking(10);

        string? result1 = manager.CompleteNextInstruction();
        string? result2 = manager.CompleteNextInstruction();

        Assert.Equal("First step", result1);
        Assert.Equal("Second step", result2);
    }

    [Fact]
    public void CompleteNextInstruction_ReturnNull_WhenNextInstructionUnAvailable()
    {
        var manager = CreateManager();
        manager.StartCooking(10);
        manager.CompleteNextInstruction();
        manager.CompleteNextInstruction();

        string? result = manager.CompleteNextInstruction();

        Assert.Null(result);
    }

    [Fact]
    public void RecipeManagerIntegration_UpdateRecipeAndCook()
    {
        var manager = CreateManager();
        Recipe testRecipe = new()
        {
            Id = 20,
            Title = "Recipe B - Updated",
            Ingredients = ["5 cups of Sugar", "1 cup of Milk"],
            Instructions = ["First step", "Second step"]
        };

        // To test the Recipe manager by acting out an updated recipe, adding to plan and then using instructions.
        var testFindOriginalRecipe = manager.FindRecipe(20);
        var testRecipeRemoved = manager.RemoveRecipe(20);
        var testAddRecipe = manager.AddRecipe(testRecipe);
        var testVerifyRecipeAdded = manager.FindRecipe(testRecipe.Id);
        manager.AddIngredientsToShoppingList(20);
        var testShoppingList = manager.GetShoppingList();
        var testDatabaseToPlan = manager.AddRecipeToCookingPlan(20);
        var testVerifyRecipeInPlan = manager.GetCookingPlan();
        var testPlanToRemovedHistory = manager.RemoveRecipeFromCookingPlan(20);
        var testVerifyPlanInHistory = manager.PeekLastRemovedRecipe();
        var testVerifyPlanRemoved = manager.GetCookingPlan();
        var testRemovedHistoryToPlan = manager.RestoreLastRemovedRecipe();
        var testVerifyPlanReturned = manager.GetCookingPlan();
        var testStartCooking = manager.StartCooking(20);
        var testInstructions = manager.PeekNextInstruction();
        var testStepOne = manager.CompleteNextInstruction();
        var testStepTwo = manager.CompleteNextInstruction();
        var testInstructionsEmpty = manager.PeekNextInstruction();

        Assert.NotNull(testFindOriginalRecipe);
        Assert.True(testRecipeRemoved);
        Assert.True(testAddRecipe);
        Assert.NotNull(testVerifyRecipeAdded);
        Assert.Equal("Recipe B - Updated", testVerifyRecipeAdded.Title);
        Assert.Equal(["5 cups of Sugar", "1 cup of Milk"], testShoppingList);
        Assert.True(testDatabaseToPlan);
        Assert.Contains(20, testVerifyRecipeInPlan);
        Assert.True(testPlanToRemovedHistory);
        Assert.Equal(testRecipe.Id, testVerifyPlanInHistory);
        Assert.DoesNotContain(testRecipe.Id, testVerifyPlanRemoved);
        Assert.True(testRemovedHistoryToPlan);
        Assert.Contains(testRecipe.Id, testVerifyPlanReturned);
        Assert.True(testStartCooking);
        Assert.Equal("First step", testInstructions);
        Assert.Equal("First step", testStepOne);
        Assert.Equal("Second step", testStepTwo);
        Assert.Null(testInstructionsEmpty);
    }
}
