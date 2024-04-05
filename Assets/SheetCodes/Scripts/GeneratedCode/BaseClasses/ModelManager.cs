using System;
using System.Collections.Generic;
using UnityEngine;

namespace SheetCodes
{
	//Generated code, do not edit!

	public static class ModelManager
	{
        private static Dictionary<DatasheetType, LoadRequest> loadRequests;

        static ModelManager()
        {
            loadRequests = new Dictionary<DatasheetType, LoadRequest>();
        }

        public static void InitializeAll()
        {
            DatasheetType[] values = Enum.GetValues(typeof(DatasheetType)) as DatasheetType[];
            foreach(DatasheetType value in values)
                Initialize(value);
        }
		
        public static void Unload(DatasheetType datasheetType)
        {
            switch (datasheetType)
            {
                case DatasheetType.Enemy:
                    {
                        if (enemyModel == null || enemyModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(enemyModel);
                        enemyModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Enemy, out request))
                        {
                            loadRequests.Remove(DatasheetType.Enemy);
                            request.resourceRequest.completed -= OnLoadCompleted_EnemyModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.Particle:
                    {
                        if (particleModel == null || particleModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(particleModel);
                        particleModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Particle, out request))
                        {
                            loadRequests.Remove(DatasheetType.Particle);
                            request.resourceRequest.completed -= OnLoadCompleted_ParticleModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.Item:
                    {
                        if (itemModel == null || itemModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(itemModel);
                        itemModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Item, out request))
                        {
                            loadRequests.Remove(DatasheetType.Item);
                            request.resourceRequest.completed -= OnLoadCompleted_ItemModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.ItemDrop:
                    {
                        if (itemDropModel == null || itemDropModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(itemDropModel);
                        itemDropModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ItemDrop, out request))
                        {
                            loadRequests.Remove(DatasheetType.ItemDrop);
                            request.resourceRequest.completed -= OnLoadCompleted_ItemDropModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.WorldResource:
                    {
                        if (worldResourceModel == null || worldResourceModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(worldResourceModel);
                        worldResourceModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.WorldResource, out request))
                        {
                            loadRequests.Remove(DatasheetType.WorldResource);
                            request.resourceRequest.completed -= OnLoadCompleted_WorldResourceModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.Biome:
                    {
                        if (biomeModel == null || biomeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(biomeModel);
                        biomeModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Biome, out request))
                        {
                            loadRequests.Remove(DatasheetType.Biome);
                            request.resourceRequest.completed -= OnLoadCompleted_BiomeModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.BiomeLayer:
                    {
                        if (biomeLayerModel == null || biomeLayerModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(biomeLayerModel);
                        biomeLayerModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.BiomeLayer, out request))
                        {
                            loadRequests.Remove(DatasheetType.BiomeLayer);
                            request.resourceRequest.completed -= OnLoadCompleted_BiomeLayerModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.Room:
                    {
                        if (roomModel == null || roomModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(roomModel);
                        roomModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Room, out request))
                        {
                            loadRequests.Remove(DatasheetType.Room);
                            request.resourceRequest.completed -= OnLoadCompleted_RoomModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.ResourceSpawn:
                    {
                        if (resourceSpawnModel == null || resourceSpawnModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(resourceSpawnModel);
                        resourceSpawnModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ResourceSpawn, out request))
                        {
                            loadRequests.Remove(DatasheetType.ResourceSpawn);
                            request.resourceRequest.completed -= OnLoadCompleted_ResourceSpawnModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.ResourceSpawnWeight:
                    {
                        if (resourceSpawnWeightModel == null || resourceSpawnWeightModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(resourceSpawnWeightModel);
                        resourceSpawnWeightModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ResourceSpawnWeight, out request))
                        {
                            loadRequests.Remove(DatasheetType.ResourceSpawnWeight);
                            request.resourceRequest.completed -= OnLoadCompleted_ResourceSpawnWeightModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.ResourceType:
                    {
                        if (resourceTypeModel == null || resourceTypeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(resourceTypeModel);
                        resourceTypeModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ResourceType, out request))
                        {
                            loadRequests.Remove(DatasheetType.ResourceType);
                            request.resourceRequest.completed -= OnLoadCompleted_ResourceTypeModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.ComputeShader:
                    {
                        if (computeShaderModel == null || computeShaderModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(computeShaderModel);
                        computeShaderModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ComputeShader, out request))
                        {
                            loadRequests.Remove(DatasheetType.ComputeShader);
                            request.resourceRequest.completed -= OnLoadCompleted_ComputeShaderModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.ItemCategory:
                    {
                        if (itemCategoryModel == null || itemCategoryModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(itemCategoryModel);
                        itemCategoryModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ItemCategory, out request))
                        {
                            loadRequests.Remove(DatasheetType.ItemCategory);
                            request.resourceRequest.completed -= OnLoadCompleted_ItemCategoryModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.CraftingRecipe:
                    {
                        if (craftingRecipeModel == null || craftingRecipeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(craftingRecipeModel);
                        craftingRecipeModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CraftingRecipe, out request))
                        {
                            loadRequests.Remove(DatasheetType.CraftingRecipe);
                            request.resourceRequest.completed -= OnLoadCompleted_CraftingRecipeModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.CraftingCategory:
                    {
                        if (craftingCategoryModel == null || craftingCategoryModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(craftingCategoryModel);
                        craftingCategoryModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CraftingCategory, out request))
                        {
                            loadRequests.Remove(DatasheetType.CraftingCategory);
                            request.resourceRequest.completed -= OnLoadCompleted_CraftingCategoryModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.CraftingInput:
                    {
                        if (craftingInputModel == null || craftingInputModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(craftingInputModel);
                        craftingInputModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CraftingInput, out request))
                        {
                            loadRequests.Remove(DatasheetType.CraftingInput);
                            request.resourceRequest.completed -= OnLoadCompleted_CraftingInputModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.CraftingOutput:
                    {
                        if (craftingOutputModel == null || craftingOutputModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(craftingOutputModel);
                        craftingOutputModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CraftingOutput, out request))
                        {
                            loadRequests.Remove(DatasheetType.CraftingOutput);
                            request.resourceRequest.completed -= OnLoadCompleted_CraftingOutputModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.ItemQuality:
                    {
                        if (itemQualityModel == null || itemQualityModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(itemQualityModel);
                        itemQualityModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ItemQuality, out request))
                        {
                            loadRequests.Remove(DatasheetType.ItemQuality);
                            request.resourceRequest.completed -= OnLoadCompleted_ItemQualityModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.ToolBaseStats:
                    {
                        if (toolBaseStatsModel == null || toolBaseStatsModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(toolBaseStatsModel);
                        toolBaseStatsModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ToolBaseStats, out request))
                        {
                            loadRequests.Remove(DatasheetType.ToolBaseStats);
                            request.resourceRequest.completed -= OnLoadCompleted_ToolBaseStatsModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.PrefabCollection:
                    {
                        if (prefabCollectionModel == null || prefabCollectionModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(prefabCollectionModel);
                        prefabCollectionModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.PrefabCollection, out request))
                        {
                            loadRequests.Remove(DatasheetType.PrefabCollection);
                            request.resourceRequest.completed -= OnLoadCompleted_PrefabCollectionModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.Token:
                    {
                        if (tokenModel == null || tokenModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(tokenModel);
                        tokenModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Token, out request))
                        {
                            loadRequests.Remove(DatasheetType.Token);
                            request.resourceRequest.completed -= OnLoadCompleted_TokenModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.RoomMaterials:
                    {
                        if (roomMaterialsModel == null || roomMaterialsModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(roomMaterialsModel);
                        roomMaterialsModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.RoomMaterials, out request))
                        {
                            loadRequests.Remove(DatasheetType.RoomMaterials);
                            request.resourceRequest.completed -= OnLoadCompleted_RoomMaterialsModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                case DatasheetType.CellMaterial:
                    {
                        if (cellMaterialModel == null || cellMaterialModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to unload model {0}. Model is not loaded.", datasheetType));
                            break;
                        }
                        Resources.UnloadAsset(cellMaterialModel);
                        cellMaterialModel = null;
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CellMaterial, out request))
                        {
                            loadRequests.Remove(DatasheetType.CellMaterial);
                            request.resourceRequest.completed -= OnLoadCompleted_CellMaterialModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(false);
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        public static void Initialize(DatasheetType datasheetType)
        {
            switch (datasheetType)
            {
                case DatasheetType.Enemy:
                    {
                        if (enemyModel != null && !enemyModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        enemyModel = Resources.Load<EnemyModel>("ScriptableObjects/Enemy");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Enemy, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.Enemy);
                            request.resourceRequest.completed -= OnLoadCompleted_EnemyModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.Particle:
                    {
                        if (particleModel != null && !particleModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        particleModel = Resources.Load<ParticleModel>("ScriptableObjects/Particle");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Particle, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.Particle);
                            request.resourceRequest.completed -= OnLoadCompleted_ParticleModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.Item:
                    {
                        if (itemModel != null && !itemModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        itemModel = Resources.Load<ItemModel>("ScriptableObjects/Item");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Item, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.Item);
                            request.resourceRequest.completed -= OnLoadCompleted_ItemModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.ItemDrop:
                    {
                        if (itemDropModel != null && !itemDropModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        itemDropModel = Resources.Load<ItemDropModel>("ScriptableObjects/ItemDrop");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ItemDrop, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.ItemDrop);
                            request.resourceRequest.completed -= OnLoadCompleted_ItemDropModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.WorldResource:
                    {
                        if (worldResourceModel != null && !worldResourceModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        worldResourceModel = Resources.Load<WorldResourceModel>("ScriptableObjects/WorldResource");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.WorldResource, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.WorldResource);
                            request.resourceRequest.completed -= OnLoadCompleted_WorldResourceModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.Biome:
                    {
                        if (biomeModel != null && !biomeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        biomeModel = Resources.Load<BiomeModel>("ScriptableObjects/Biome");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Biome, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.Biome);
                            request.resourceRequest.completed -= OnLoadCompleted_BiomeModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.BiomeLayer:
                    {
                        if (biomeLayerModel != null && !biomeLayerModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        biomeLayerModel = Resources.Load<BiomeLayerModel>("ScriptableObjects/BiomeLayer");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.BiomeLayer, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.BiomeLayer);
                            request.resourceRequest.completed -= OnLoadCompleted_BiomeLayerModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.Room:
                    {
                        if (roomModel != null && !roomModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        roomModel = Resources.Load<RoomModel>("ScriptableObjects/Room");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Room, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.Room);
                            request.resourceRequest.completed -= OnLoadCompleted_RoomModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.ResourceSpawn:
                    {
                        if (resourceSpawnModel != null && !resourceSpawnModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        resourceSpawnModel = Resources.Load<ResourceSpawnModel>("ScriptableObjects/ResourceSpawn");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ResourceSpawn, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.ResourceSpawn);
                            request.resourceRequest.completed -= OnLoadCompleted_ResourceSpawnModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.ResourceSpawnWeight:
                    {
                        if (resourceSpawnWeightModel != null && !resourceSpawnWeightModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        resourceSpawnWeightModel = Resources.Load<ResourceSpawnWeightModel>("ScriptableObjects/ResourceSpawnWeight");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ResourceSpawnWeight, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.ResourceSpawnWeight);
                            request.resourceRequest.completed -= OnLoadCompleted_ResourceSpawnWeightModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.ResourceType:
                    {
                        if (resourceTypeModel != null && !resourceTypeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        resourceTypeModel = Resources.Load<ResourceTypeModel>("ScriptableObjects/ResourceType");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ResourceType, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.ResourceType);
                            request.resourceRequest.completed -= OnLoadCompleted_ResourceTypeModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.ComputeShader:
                    {
                        if (computeShaderModel != null && !computeShaderModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        computeShaderModel = Resources.Load<ComputeShaderModel>("ScriptableObjects/ComputeShader");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ComputeShader, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.ComputeShader);
                            request.resourceRequest.completed -= OnLoadCompleted_ComputeShaderModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.ItemCategory:
                    {
                        if (itemCategoryModel != null && !itemCategoryModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        itemCategoryModel = Resources.Load<ItemCategoryModel>("ScriptableObjects/ItemCategory");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ItemCategory, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.ItemCategory);
                            request.resourceRequest.completed -= OnLoadCompleted_ItemCategoryModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.CraftingRecipe:
                    {
                        if (craftingRecipeModel != null && !craftingRecipeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        craftingRecipeModel = Resources.Load<CraftingRecipeModel>("ScriptableObjects/CraftingRecipe");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CraftingRecipe, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.CraftingRecipe);
                            request.resourceRequest.completed -= OnLoadCompleted_CraftingRecipeModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.CraftingCategory:
                    {
                        if (craftingCategoryModel != null && !craftingCategoryModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        craftingCategoryModel = Resources.Load<CraftingCategoryModel>("ScriptableObjects/CraftingCategory");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CraftingCategory, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.CraftingCategory);
                            request.resourceRequest.completed -= OnLoadCompleted_CraftingCategoryModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.CraftingInput:
                    {
                        if (craftingInputModel != null && !craftingInputModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        craftingInputModel = Resources.Load<CraftingInputModel>("ScriptableObjects/CraftingInput");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CraftingInput, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.CraftingInput);
                            request.resourceRequest.completed -= OnLoadCompleted_CraftingInputModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.CraftingOutput:
                    {
                        if (craftingOutputModel != null && !craftingOutputModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        craftingOutputModel = Resources.Load<CraftingOutputModel>("ScriptableObjects/CraftingOutput");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CraftingOutput, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.CraftingOutput);
                            request.resourceRequest.completed -= OnLoadCompleted_CraftingOutputModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.ItemQuality:
                    {
                        if (itemQualityModel != null && !itemQualityModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        itemQualityModel = Resources.Load<ItemQualityModel>("ScriptableObjects/ItemQuality");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ItemQuality, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.ItemQuality);
                            request.resourceRequest.completed -= OnLoadCompleted_ItemQualityModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.ToolBaseStats:
                    {
                        if (toolBaseStatsModel != null && !toolBaseStatsModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        toolBaseStatsModel = Resources.Load<ToolBaseStatsModel>("ScriptableObjects/ToolBaseStats");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.ToolBaseStats, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.ToolBaseStats);
                            request.resourceRequest.completed -= OnLoadCompleted_ToolBaseStatsModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.PrefabCollection:
                    {
                        if (prefabCollectionModel != null && !prefabCollectionModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        prefabCollectionModel = Resources.Load<PrefabCollectionModel>("ScriptableObjects/PrefabCollection");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.PrefabCollection, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.PrefabCollection);
                            request.resourceRequest.completed -= OnLoadCompleted_PrefabCollectionModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.Token:
                    {
                        if (tokenModel != null && !tokenModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        tokenModel = Resources.Load<TokenModel>("ScriptableObjects/Token");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.Token, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.Token);
                            request.resourceRequest.completed -= OnLoadCompleted_TokenModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.RoomMaterials:
                    {
                        if (roomMaterialsModel != null && !roomMaterialsModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        roomMaterialsModel = Resources.Load<RoomMaterialsModel>("ScriptableObjects/RoomMaterials");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.RoomMaterials, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.RoomMaterials);
                            request.resourceRequest.completed -= OnLoadCompleted_RoomMaterialsModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                case DatasheetType.CellMaterial:
                    {
                        if (cellMaterialModel != null && !cellMaterialModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to Initialize {0}. Model is already initialized.", datasheetType));
                            break;
                        }

                        cellMaterialModel = Resources.Load<CellMaterialModel>("ScriptableObjects/CellMaterial");
                        LoadRequest request;
                        if (loadRequests.TryGetValue(DatasheetType.CellMaterial, out request))
                        {
                            Log(string.Format("Sheet Codes: Trying to initialize {0} while also async loading. Async load has been canceled.", datasheetType));
                            loadRequests.Remove(DatasheetType.CellMaterial);
                            request.resourceRequest.completed -= OnLoadCompleted_CellMaterialModel;
							foreach (Action<bool> callback in request.callbacks)
								callback(true);
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        public static void InitializeAsync(DatasheetType datasheetType, Action<bool> callback)
        {
            switch (datasheetType)
            {
                case DatasheetType.Enemy:
                    {
                        if (enemyModel != null && !enemyModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.Enemy))
                        {
                            loadRequests[DatasheetType.Enemy].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<EnemyModel>("ScriptableObjects/Enemy");
                        loadRequests.Add(DatasheetType.Enemy, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_EnemyModel;
                        break;
                    }
                case DatasheetType.Particle:
                    {
                        if (particleModel != null && !particleModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.Particle))
                        {
                            loadRequests[DatasheetType.Particle].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ParticleModel>("ScriptableObjects/Particle");
                        loadRequests.Add(DatasheetType.Particle, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ParticleModel;
                        break;
                    }
                case DatasheetType.Item:
                    {
                        if (itemModel != null && !itemModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.Item))
                        {
                            loadRequests[DatasheetType.Item].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ItemModel>("ScriptableObjects/Item");
                        loadRequests.Add(DatasheetType.Item, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ItemModel;
                        break;
                    }
                case DatasheetType.ItemDrop:
                    {
                        if (itemDropModel != null && !itemDropModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.ItemDrop))
                        {
                            loadRequests[DatasheetType.ItemDrop].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ItemDropModel>("ScriptableObjects/ItemDrop");
                        loadRequests.Add(DatasheetType.ItemDrop, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ItemDropModel;
                        break;
                    }
                case DatasheetType.WorldResource:
                    {
                        if (worldResourceModel != null && !worldResourceModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.WorldResource))
                        {
                            loadRequests[DatasheetType.WorldResource].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<WorldResourceModel>("ScriptableObjects/WorldResource");
                        loadRequests.Add(DatasheetType.WorldResource, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_WorldResourceModel;
                        break;
                    }
                case DatasheetType.Biome:
                    {
                        if (biomeModel != null && !biomeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.Biome))
                        {
                            loadRequests[DatasheetType.Biome].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<BiomeModel>("ScriptableObjects/Biome");
                        loadRequests.Add(DatasheetType.Biome, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_BiomeModel;
                        break;
                    }
                case DatasheetType.BiomeLayer:
                    {
                        if (biomeLayerModel != null && !biomeLayerModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.BiomeLayer))
                        {
                            loadRequests[DatasheetType.BiomeLayer].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<BiomeLayerModel>("ScriptableObjects/BiomeLayer");
                        loadRequests.Add(DatasheetType.BiomeLayer, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_BiomeLayerModel;
                        break;
                    }
                case DatasheetType.Room:
                    {
                        if (roomModel != null && !roomModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.Room))
                        {
                            loadRequests[DatasheetType.Room].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<RoomModel>("ScriptableObjects/Room");
                        loadRequests.Add(DatasheetType.Room, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_RoomModel;
                        break;
                    }
                case DatasheetType.ResourceSpawn:
                    {
                        if (resourceSpawnModel != null && !resourceSpawnModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.ResourceSpawn))
                        {
                            loadRequests[DatasheetType.ResourceSpawn].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ResourceSpawnModel>("ScriptableObjects/ResourceSpawn");
                        loadRequests.Add(DatasheetType.ResourceSpawn, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ResourceSpawnModel;
                        break;
                    }
                case DatasheetType.ResourceSpawnWeight:
                    {
                        if (resourceSpawnWeightModel != null && !resourceSpawnWeightModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.ResourceSpawnWeight))
                        {
                            loadRequests[DatasheetType.ResourceSpawnWeight].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ResourceSpawnWeightModel>("ScriptableObjects/ResourceSpawnWeight");
                        loadRequests.Add(DatasheetType.ResourceSpawnWeight, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ResourceSpawnWeightModel;
                        break;
                    }
                case DatasheetType.ResourceType:
                    {
                        if (resourceTypeModel != null && !resourceTypeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.ResourceType))
                        {
                            loadRequests[DatasheetType.ResourceType].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ResourceTypeModel>("ScriptableObjects/ResourceType");
                        loadRequests.Add(DatasheetType.ResourceType, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ResourceTypeModel;
                        break;
                    }
                case DatasheetType.ComputeShader:
                    {
                        if (computeShaderModel != null && !computeShaderModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.ComputeShader))
                        {
                            loadRequests[DatasheetType.ComputeShader].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ComputeShaderModel>("ScriptableObjects/ComputeShader");
                        loadRequests.Add(DatasheetType.ComputeShader, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ComputeShaderModel;
                        break;
                    }
                case DatasheetType.ItemCategory:
                    {
                        if (itemCategoryModel != null && !itemCategoryModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.ItemCategory))
                        {
                            loadRequests[DatasheetType.ItemCategory].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ItemCategoryModel>("ScriptableObjects/ItemCategory");
                        loadRequests.Add(DatasheetType.ItemCategory, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ItemCategoryModel;
                        break;
                    }
                case DatasheetType.CraftingRecipe:
                    {
                        if (craftingRecipeModel != null && !craftingRecipeModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.CraftingRecipe))
                        {
                            loadRequests[DatasheetType.CraftingRecipe].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<CraftingRecipeModel>("ScriptableObjects/CraftingRecipe");
                        loadRequests.Add(DatasheetType.CraftingRecipe, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_CraftingRecipeModel;
                        break;
                    }
                case DatasheetType.CraftingCategory:
                    {
                        if (craftingCategoryModel != null && !craftingCategoryModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.CraftingCategory))
                        {
                            loadRequests[DatasheetType.CraftingCategory].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<CraftingCategoryModel>("ScriptableObjects/CraftingCategory");
                        loadRequests.Add(DatasheetType.CraftingCategory, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_CraftingCategoryModel;
                        break;
                    }
                case DatasheetType.CraftingInput:
                    {
                        if (craftingInputModel != null && !craftingInputModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.CraftingInput))
                        {
                            loadRequests[DatasheetType.CraftingInput].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<CraftingInputModel>("ScriptableObjects/CraftingInput");
                        loadRequests.Add(DatasheetType.CraftingInput, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_CraftingInputModel;
                        break;
                    }
                case DatasheetType.CraftingOutput:
                    {
                        if (craftingOutputModel != null && !craftingOutputModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.CraftingOutput))
                        {
                            loadRequests[DatasheetType.CraftingOutput].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<CraftingOutputModel>("ScriptableObjects/CraftingOutput");
                        loadRequests.Add(DatasheetType.CraftingOutput, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_CraftingOutputModel;
                        break;
                    }
                case DatasheetType.ItemQuality:
                    {
                        if (itemQualityModel != null && !itemQualityModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.ItemQuality))
                        {
                            loadRequests[DatasheetType.ItemQuality].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ItemQualityModel>("ScriptableObjects/ItemQuality");
                        loadRequests.Add(DatasheetType.ItemQuality, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ItemQualityModel;
                        break;
                    }
                case DatasheetType.ToolBaseStats:
                    {
                        if (toolBaseStatsModel != null && !toolBaseStatsModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.ToolBaseStats))
                        {
                            loadRequests[DatasheetType.ToolBaseStats].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<ToolBaseStatsModel>("ScriptableObjects/ToolBaseStats");
                        loadRequests.Add(DatasheetType.ToolBaseStats, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_ToolBaseStatsModel;
                        break;
                    }
                case DatasheetType.PrefabCollection:
                    {
                        if (prefabCollectionModel != null && !prefabCollectionModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.PrefabCollection))
                        {
                            loadRequests[DatasheetType.PrefabCollection].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<PrefabCollectionModel>("ScriptableObjects/PrefabCollection");
                        loadRequests.Add(DatasheetType.PrefabCollection, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_PrefabCollectionModel;
                        break;
                    }
                case DatasheetType.Token:
                    {
                        if (tokenModel != null && !tokenModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.Token))
                        {
                            loadRequests[DatasheetType.Token].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<TokenModel>("ScriptableObjects/Token");
                        loadRequests.Add(DatasheetType.Token, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_TokenModel;
                        break;
                    }
                case DatasheetType.RoomMaterials:
                    {
                        if (roomMaterialsModel != null && !roomMaterialsModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.RoomMaterials))
                        {
                            loadRequests[DatasheetType.RoomMaterials].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<RoomMaterialsModel>("ScriptableObjects/RoomMaterials");
                        loadRequests.Add(DatasheetType.RoomMaterials, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_RoomMaterialsModel;
                        break;
                    }
                case DatasheetType.CellMaterial:
                    {
                        if (cellMaterialModel != null && !cellMaterialModel.Equals(null))
                        {
                            Log(string.Format("Sheet Codes: Trying to InitializeAsync {0}. Model is already initialized.", datasheetType));
                            callback(true);
                            break;
                        }
                        if(loadRequests.ContainsKey(DatasheetType.CellMaterial))
                        {
                            loadRequests[DatasheetType.CellMaterial].callbacks.Add(callback);
                            break;
                        }
                        ResourceRequest request = Resources.LoadAsync<CellMaterialModel>("ScriptableObjects/CellMaterial");
                        loadRequests.Add(DatasheetType.CellMaterial, new LoadRequest(request, callback));
                        request.completed += OnLoadCompleted_CellMaterialModel;
                        break;
                    }
                default:
                    break;
            }
        }

        private static void OnLoadCompleted_EnemyModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.Enemy];
            enemyModel = request.resourceRequest.asset as EnemyModel;
            loadRequests.Remove(DatasheetType.Enemy);
            operation.completed -= OnLoadCompleted_EnemyModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static EnemyModel enemyModel = default;
		public static EnemyModel EnemyModel
        {
            get
            {
                if (enemyModel == null)
                    Initialize(DatasheetType.Enemy);

                return enemyModel;
            }
        }
        private static void OnLoadCompleted_ParticleModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.Particle];
            particleModel = request.resourceRequest.asset as ParticleModel;
            loadRequests.Remove(DatasheetType.Particle);
            operation.completed -= OnLoadCompleted_ParticleModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ParticleModel particleModel = default;
		public static ParticleModel ParticleModel
        {
            get
            {
                if (particleModel == null)
                    Initialize(DatasheetType.Particle);

                return particleModel;
            }
        }
        private static void OnLoadCompleted_ItemModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.Item];
            itemModel = request.resourceRequest.asset as ItemModel;
            loadRequests.Remove(DatasheetType.Item);
            operation.completed -= OnLoadCompleted_ItemModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ItemModel itemModel = default;
		public static ItemModel ItemModel
        {
            get
            {
                if (itemModel == null)
                    Initialize(DatasheetType.Item);

                return itemModel;
            }
        }
        private static void OnLoadCompleted_ItemDropModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.ItemDrop];
            itemDropModel = request.resourceRequest.asset as ItemDropModel;
            loadRequests.Remove(DatasheetType.ItemDrop);
            operation.completed -= OnLoadCompleted_ItemDropModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ItemDropModel itemDropModel = default;
		public static ItemDropModel ItemDropModel
        {
            get
            {
                if (itemDropModel == null)
                    Initialize(DatasheetType.ItemDrop);

                return itemDropModel;
            }
        }
        private static void OnLoadCompleted_WorldResourceModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.WorldResource];
            worldResourceModel = request.resourceRequest.asset as WorldResourceModel;
            loadRequests.Remove(DatasheetType.WorldResource);
            operation.completed -= OnLoadCompleted_WorldResourceModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static WorldResourceModel worldResourceModel = default;
		public static WorldResourceModel WorldResourceModel
        {
            get
            {
                if (worldResourceModel == null)
                    Initialize(DatasheetType.WorldResource);

                return worldResourceModel;
            }
        }
        private static void OnLoadCompleted_BiomeModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.Biome];
            biomeModel = request.resourceRequest.asset as BiomeModel;
            loadRequests.Remove(DatasheetType.Biome);
            operation.completed -= OnLoadCompleted_BiomeModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static BiomeModel biomeModel = default;
		public static BiomeModel BiomeModel
        {
            get
            {
                if (biomeModel == null)
                    Initialize(DatasheetType.Biome);

                return biomeModel;
            }
        }
        private static void OnLoadCompleted_BiomeLayerModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.BiomeLayer];
            biomeLayerModel = request.resourceRequest.asset as BiomeLayerModel;
            loadRequests.Remove(DatasheetType.BiomeLayer);
            operation.completed -= OnLoadCompleted_BiomeLayerModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static BiomeLayerModel biomeLayerModel = default;
		public static BiomeLayerModel BiomeLayerModel
        {
            get
            {
                if (biomeLayerModel == null)
                    Initialize(DatasheetType.BiomeLayer);

                return biomeLayerModel;
            }
        }
        private static void OnLoadCompleted_RoomModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.Room];
            roomModel = request.resourceRequest.asset as RoomModel;
            loadRequests.Remove(DatasheetType.Room);
            operation.completed -= OnLoadCompleted_RoomModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static RoomModel roomModel = default;
		public static RoomModel RoomModel
        {
            get
            {
                if (roomModel == null)
                    Initialize(DatasheetType.Room);

                return roomModel;
            }
        }
        private static void OnLoadCompleted_ResourceSpawnModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.ResourceSpawn];
            resourceSpawnModel = request.resourceRequest.asset as ResourceSpawnModel;
            loadRequests.Remove(DatasheetType.ResourceSpawn);
            operation.completed -= OnLoadCompleted_ResourceSpawnModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ResourceSpawnModel resourceSpawnModel = default;
		public static ResourceSpawnModel ResourceSpawnModel
        {
            get
            {
                if (resourceSpawnModel == null)
                    Initialize(DatasheetType.ResourceSpawn);

                return resourceSpawnModel;
            }
        }
        private static void OnLoadCompleted_ResourceSpawnWeightModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.ResourceSpawnWeight];
            resourceSpawnWeightModel = request.resourceRequest.asset as ResourceSpawnWeightModel;
            loadRequests.Remove(DatasheetType.ResourceSpawnWeight);
            operation.completed -= OnLoadCompleted_ResourceSpawnWeightModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ResourceSpawnWeightModel resourceSpawnWeightModel = default;
		public static ResourceSpawnWeightModel ResourceSpawnWeightModel
        {
            get
            {
                if (resourceSpawnWeightModel == null)
                    Initialize(DatasheetType.ResourceSpawnWeight);

                return resourceSpawnWeightModel;
            }
        }
        private static void OnLoadCompleted_ResourceTypeModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.ResourceType];
            resourceTypeModel = request.resourceRequest.asset as ResourceTypeModel;
            loadRequests.Remove(DatasheetType.ResourceType);
            operation.completed -= OnLoadCompleted_ResourceTypeModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ResourceTypeModel resourceTypeModel = default;
		public static ResourceTypeModel ResourceTypeModel
        {
            get
            {
                if (resourceTypeModel == null)
                    Initialize(DatasheetType.ResourceType);

                return resourceTypeModel;
            }
        }
        private static void OnLoadCompleted_ComputeShaderModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.ComputeShader];
            computeShaderModel = request.resourceRequest.asset as ComputeShaderModel;
            loadRequests.Remove(DatasheetType.ComputeShader);
            operation.completed -= OnLoadCompleted_ComputeShaderModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ComputeShaderModel computeShaderModel = default;
		public static ComputeShaderModel ComputeShaderModel
        {
            get
            {
                if (computeShaderModel == null)
                    Initialize(DatasheetType.ComputeShader);

                return computeShaderModel;
            }
        }
        private static void OnLoadCompleted_ItemCategoryModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.ItemCategory];
            itemCategoryModel = request.resourceRequest.asset as ItemCategoryModel;
            loadRequests.Remove(DatasheetType.ItemCategory);
            operation.completed -= OnLoadCompleted_ItemCategoryModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ItemCategoryModel itemCategoryModel = default;
		public static ItemCategoryModel ItemCategoryModel
        {
            get
            {
                if (itemCategoryModel == null)
                    Initialize(DatasheetType.ItemCategory);

                return itemCategoryModel;
            }
        }
        private static void OnLoadCompleted_CraftingRecipeModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.CraftingRecipe];
            craftingRecipeModel = request.resourceRequest.asset as CraftingRecipeModel;
            loadRequests.Remove(DatasheetType.CraftingRecipe);
            operation.completed -= OnLoadCompleted_CraftingRecipeModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static CraftingRecipeModel craftingRecipeModel = default;
		public static CraftingRecipeModel CraftingRecipeModel
        {
            get
            {
                if (craftingRecipeModel == null)
                    Initialize(DatasheetType.CraftingRecipe);

                return craftingRecipeModel;
            }
        }
        private static void OnLoadCompleted_CraftingCategoryModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.CraftingCategory];
            craftingCategoryModel = request.resourceRequest.asset as CraftingCategoryModel;
            loadRequests.Remove(DatasheetType.CraftingCategory);
            operation.completed -= OnLoadCompleted_CraftingCategoryModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static CraftingCategoryModel craftingCategoryModel = default;
		public static CraftingCategoryModel CraftingCategoryModel
        {
            get
            {
                if (craftingCategoryModel == null)
                    Initialize(DatasheetType.CraftingCategory);

                return craftingCategoryModel;
            }
        }
        private static void OnLoadCompleted_CraftingInputModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.CraftingInput];
            craftingInputModel = request.resourceRequest.asset as CraftingInputModel;
            loadRequests.Remove(DatasheetType.CraftingInput);
            operation.completed -= OnLoadCompleted_CraftingInputModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static CraftingInputModel craftingInputModel = default;
		public static CraftingInputModel CraftingInputModel
        {
            get
            {
                if (craftingInputModel == null)
                    Initialize(DatasheetType.CraftingInput);

                return craftingInputModel;
            }
        }
        private static void OnLoadCompleted_CraftingOutputModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.CraftingOutput];
            craftingOutputModel = request.resourceRequest.asset as CraftingOutputModel;
            loadRequests.Remove(DatasheetType.CraftingOutput);
            operation.completed -= OnLoadCompleted_CraftingOutputModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static CraftingOutputModel craftingOutputModel = default;
		public static CraftingOutputModel CraftingOutputModel
        {
            get
            {
                if (craftingOutputModel == null)
                    Initialize(DatasheetType.CraftingOutput);

                return craftingOutputModel;
            }
        }
        private static void OnLoadCompleted_ItemQualityModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.ItemQuality];
            itemQualityModel = request.resourceRequest.asset as ItemQualityModel;
            loadRequests.Remove(DatasheetType.ItemQuality);
            operation.completed -= OnLoadCompleted_ItemQualityModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ItemQualityModel itemQualityModel = default;
		public static ItemQualityModel ItemQualityModel
        {
            get
            {
                if (itemQualityModel == null)
                    Initialize(DatasheetType.ItemQuality);

                return itemQualityModel;
            }
        }
        private static void OnLoadCompleted_ToolBaseStatsModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.ToolBaseStats];
            toolBaseStatsModel = request.resourceRequest.asset as ToolBaseStatsModel;
            loadRequests.Remove(DatasheetType.ToolBaseStats);
            operation.completed -= OnLoadCompleted_ToolBaseStatsModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static ToolBaseStatsModel toolBaseStatsModel = default;
		public static ToolBaseStatsModel ToolBaseStatsModel
        {
            get
            {
                if (toolBaseStatsModel == null)
                    Initialize(DatasheetType.ToolBaseStats);

                return toolBaseStatsModel;
            }
        }
        private static void OnLoadCompleted_PrefabCollectionModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.PrefabCollection];
            prefabCollectionModel = request.resourceRequest.asset as PrefabCollectionModel;
            loadRequests.Remove(DatasheetType.PrefabCollection);
            operation.completed -= OnLoadCompleted_PrefabCollectionModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static PrefabCollectionModel prefabCollectionModel = default;
		public static PrefabCollectionModel PrefabCollectionModel
        {
            get
            {
                if (prefabCollectionModel == null)
                    Initialize(DatasheetType.PrefabCollection);

                return prefabCollectionModel;
            }
        }
        private static void OnLoadCompleted_TokenModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.Token];
            tokenModel = request.resourceRequest.asset as TokenModel;
            loadRequests.Remove(DatasheetType.Token);
            operation.completed -= OnLoadCompleted_TokenModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static TokenModel tokenModel = default;
		public static TokenModel TokenModel
        {
            get
            {
                if (tokenModel == null)
                    Initialize(DatasheetType.Token);

                return tokenModel;
            }
        }
        private static void OnLoadCompleted_RoomMaterialsModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.RoomMaterials];
            roomMaterialsModel = request.resourceRequest.asset as RoomMaterialsModel;
            loadRequests.Remove(DatasheetType.RoomMaterials);
            operation.completed -= OnLoadCompleted_RoomMaterialsModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static RoomMaterialsModel roomMaterialsModel = default;
		public static RoomMaterialsModel RoomMaterialsModel
        {
            get
            {
                if (roomMaterialsModel == null)
                    Initialize(DatasheetType.RoomMaterials);

                return roomMaterialsModel;
            }
        }
        private static void OnLoadCompleted_CellMaterialModel(AsyncOperation operation)
        {
            LoadRequest request = loadRequests[DatasheetType.CellMaterial];
            cellMaterialModel = request.resourceRequest.asset as CellMaterialModel;
            loadRequests.Remove(DatasheetType.CellMaterial);
            operation.completed -= OnLoadCompleted_CellMaterialModel;
            foreach (Action<bool> callback in request.callbacks)
                callback(true);
        }

		private static CellMaterialModel cellMaterialModel = default;
		public static CellMaterialModel CellMaterialModel
        {
            get
            {
                if (cellMaterialModel == null)
                    Initialize(DatasheetType.CellMaterial);

                return cellMaterialModel;
            }
        }
		
        private static void Log(string message)
        {
            Debug.LogWarning(message);
        }
	}
	
    public struct LoadRequest
    {
        public readonly ResourceRequest resourceRequest;
        public readonly List<Action<bool>> callbacks;

        public LoadRequest(ResourceRequest resourceRequest, Action<bool> callback)
        {
            this.resourceRequest = resourceRequest;
            callbacks = new List<Action<bool>>() { callback };
        }
    }
}
