using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace MapReader {
	public class MapLoader : MonoBehaviour {

		public static string SPRITES_PATH = "Assets/Resources/";

        public Material pixelSnapMaterial;
		public TextAsset mapTMX;
		public MapXml map;
		public float scale;
        public bool updateCameraConstraints = false;
		GameObject mapObject;

		public void LoadMap() {
			// Reading the XML document
			XmlDocument xmlDocument = new XmlDocument ();
			xmlDocument.LoadXml (mapTMX.text);

			// Creates the map with the document
			map = new MapXml (xmlDocument);

			// In case another Map was loaded, deletes it
            string mapTitle = map.properties["Title"];
            if (mapTitle == null)
                mapTitle = "Map";

			Transform mapTransform = transform.FindChild (mapTitle);
			if (mapTransform != null)
				mapObject = mapTransform.gameObject;
				
			if (mapObject != null) {
				if (mapObject.name ==  mapTitle) {
					DestroyImmediate(mapObject);
				}
			}
			
			// Instantiates the container object where the tiles will be put in
			mapObject = new GameObject(name);
			mapObject.name = mapTitle;
			mapObject.transform.parent = transform;

            //Map mapScript = mapObject.AddComponent<Map>();
            //mapScript.name = mapTitle;
            //mapScript.clampsCamera = map.GetProperty("clamps") != null ? bool.Parse(map.GetProperty("clamps")) : false;
            //mapScript.mapXml = map;

			// Makes the magic!11!11!
			drawLayers();
			instantiateObjects();
		}

		// Draws the tiles inside the Map object we set earlier
		void drawLayers() {
			int tileWidth = map.tileWidth;
			int tileHeight = map.tileHeight;
			
			List<Layer> layers = map.layers;
			
			int layerHeight = 0;
			foreach (Layer layer in layers) {
				GameObject layerObject = new GameObject(layer.name);

				layerObject.transform.position = new Vector3(0, 0, layerHeight);
				layerObject.transform.parent = mapObject.transform;
				
				for (int y = 0; y < layer.layerTiles.Count; y++) {
					List<LayerTile> xTiles = layer.layerTiles[y];
					for (int x = 0; x < xTiles.Count; x++) {
						LayerTile layerTile = xTiles[x];
						int gid = layerTile.gid;
						
						if (gid == 0)
							continue;
						
						// Gets the right tileset and texture for this gid
						Tileset tileset = map.GetTileset (gid);
						Texture2D texture = tileset.texture;
						if (tileset == null || texture == null)
							continue;
						
						Tile tile = tileset.tiles[gid];
						Rect rect = tile.area;
						
						Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), scale);
						GameObject tileObject = new GameObject("tile-" + x + "-" + y);
						tileObject.transform.parent = layerObject.transform;

						float xCoordinate = (float)((x * tileWidth) / scale);
						float yCoordinate = (float)((y * tileHeight) / scale);
						
						int xScale = 1;
						if (layerTile.flippedH) {
							xScale = -1;
							xCoordinate += (float)(tileWidth / scale);
						}
						
						int yScale = 1; 
						if (layerTile.flippedV) {
							yScale = -1;
							yCoordinate += (float)(tileHeight / scale);
						}
						
						if (layerTile.flippedD) {
							tileObject.transform.Rotate (new Vector3(0, 0, -90));
							yCoordinate -= (float)(tileHeight / scale);
						}
						
						tileObject.transform.localScale = new Vector3(xScale, yScale, 0);
						tileObject.transform.position = new Vector3(xCoordinate, yCoordinate, layerHeight);

                        SpriteRenderer renderer = tileObject.AddComponent<SpriteRenderer>();
                        renderer.sprite = sprite;
						renderer.sortingLayerName = layer.name;
                        renderer.material = pixelSnapMaterial;

					}
				}
                layerHeight -= 5;
			}
		}

		// Instantiates the collisions
		void instantiateObjects() {
			List<ObjectGroup> objectGroups = map.objectGroups;

			foreach (ObjectGroup objectGroup in objectGroups) {
                GameObject folderObject = new GameObject(objectGroup.name);
                folderObject.transform.parent = mapObject.transform;

                foreach (MapObject someMapObject in objectGroup.objects) {
                    GameObject newObject = ObjectFactory.InstantiateObject(objectGroup, someMapObject, scale);
                    if (newObject != null)
                    {
                        newObject.transform.position = new Vector3((someMapObject.x / scale), ((map.height * map.tileHeight - someMapObject.y - someMapObject.height) / scale), -5);
                        newObject.transform.parent = folderObject.transform;
                    }
                }
			}
		}
	}
}
