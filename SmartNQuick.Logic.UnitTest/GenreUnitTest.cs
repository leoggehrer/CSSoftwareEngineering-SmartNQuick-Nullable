using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartNQuick.Contracts.Persistence.MusicStore;
using System;
using System.Threading.Tasks;

namespace SmartNQuick.Logic.UnitTest
{
	[TestClass]
	public class GenreUnitTest
	{
		private static SmartNQuick.Contracts.Client.IControllerAccess<IGenre> CreateController()
		{
			return Logic.Factory.Create<IGenre>();
		}
		private static async void DeleteAllGenresAsync()
		{
			using var ctrl = CreateController();

			foreach (var item in await ctrl.GetAllAsync())
			{
				await ctrl.DeleteAsync(item.Id);
			}
			await ctrl.SaveChangesAsync();
		}
		[ClassInitialize]
#pragma warning disable IDE0060 // Remove unused parameter
		public static void ClassInitialize(TestContext context)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			DeleteAllGenresAsync();
		}
		[ClassCleanup]
		public static void ClassCleanup()
		{
			DeleteAllGenresAsync();
		}
		[TestInitialize]
		public void TestInitialize()
		{

		}
		[TestCleanup]
		public void TestCleanup()
		{

		}
		[TestMethod]
		public void Create_NoneRequirements_Result()
		{
			Task.Run(async () =>
			{
				using var ctrl = CreateController();

				var entity = await ctrl.CreateAsync();

				Assert.IsNotNull(entity);
			}).Wait();
		}
		[TestMethod]
		public void Insert_WithUniqueName_ResultPersistenceEntity()
		{
			IGenre expected = null;
			string expectedName = null;

			Task.Run(async () =>
			{
				using var ctrl = CreateController();
				var entity = await ctrl.CreateAsync();

				expectedName = Guid.NewGuid().ToString();
				entity.Name = expectedName;
				expected = await ctrl.InsertAsync(entity);
				await ctrl.SaveChangesAsync();
			}).Wait();
			Assert.IsNotNull(expected);
			Assert.AreNotEqual(0, expected.Id);
			Assert.AreEqual(expectedName, expected.Name);
		}
		[TestMethod]
		public void Delete_InsertItemGetItemDeleteItem_ItemNotFound()
		{
			IGenre inserted = null;
			IGenre getByIdItem = null;
			bool hasException = false;
			string name = null;

			Task.Run(async () =>
			{
				using var ctrl = CreateController();
				var entity = await ctrl.CreateAsync();

				name = Guid.NewGuid().ToString();
				entity.Name = name;
				inserted = await ctrl.InsertAsync(entity);
				await ctrl.SaveChangesAsync();

				getByIdItem = await ctrl.GetByIdAsync(inserted.Id);
				if (getByIdItem != null)
				{
					await ctrl.DeleteAsync(getByIdItem.Id);
					await ctrl.SaveChangesAsync();
				}
				try
				{
					await ctrl.GetByIdAsync(inserted.Id);
				}
				catch (Exception)
				{
					hasException = true;
				}
			}).Wait();
			Assert.IsTrue(hasException);
		}
	}
}
