using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recipes;

namespace ConstellationTest
{
    [TestClass]
    public class ConstellationTest
    {
        [TestMethod]
        public void BasicAPICallTest()
        {
            // Act
            ManageAPI.Request();

            // Assert
            Assert.IsNotNull(ManageAPI.recipesList[0].Title);
            Assert.IsNotNull(ManageAPI.recipesList[0].Instructions);
            Assert.IsNotNull(ManageAPI.recipesList[0].Glutenfree);
            Assert.IsNotNull(ManageAPI.recipesList[0].HealthScore);
            Assert.IsNotNull(ManageAPI.recipesList[0].Ingredients);
            Assert.IsNotNull(ManageAPI.recipesList[0].ReadyInMinutes);
            Assert.IsNotNull(ManageAPI.recipesList[0].SourceUrl);
            Assert.IsNotNull(ManageAPI.recipesList[0].Vegan);
            Assert.IsNotNull(ManageAPI.recipesList[0].Vegetarian);
            Assert.IsNotNull(ManageAPI.recipesList[0].Verypopular);
        }

        [TestMethod]
        public void AddRandomRecipeTest()
        {
            // Act
            ManageAPI.Request();
            ManageAPI.Request();

            // Assert
            Assert.IsNotNull(ManageAPI.recipesList[1].Title);
            Assert.IsNotNull(ManageAPI.recipesList[1].Instructions);
            Assert.IsNotNull(ManageAPI.recipesList[1].Glutenfree);
            Assert.IsNotNull(ManageAPI.recipesList[1].HealthScore);
            Assert.IsNotNull(ManageAPI.recipesList[1].Ingredients);
            Assert.IsNotNull(ManageAPI.recipesList[1].ReadyInMinutes);
            Assert.IsNotNull(ManageAPI.recipesList[1].SourceUrl);
            Assert.IsNotNull(ManageAPI.recipesList[1].Vegan);
            Assert.IsNotNull(ManageAPI.recipesList[1].Vegetarian);
            Assert.IsNotNull(ManageAPI.recipesList[1].Verypopular);
        }
    }
}
