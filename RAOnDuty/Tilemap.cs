using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace RAOnDuty {
    public class TileData {
        internal Texture2D layer1Texture;
        internal Texture2D layer2Texture;
        internal string id;
        internal string name;
        internal TileMap.TileType type;
        public TileData(Texture2D _layer2, Texture2D _layer1, string _id, string _name, TileMap.TileType _type) {
            layer2Texture = _layer2;
            layer1Texture = _layer1;
            id = _id;
            name = _name;
            type = _type;
        }
    }
    public class Tile {
        private Vector2 upperLeftCorner;
        private int SCALE;
        public TileData tileData;
        private InteractableManager interactables;
        public Tile(int X, int Y, int _scale) {
            upperLeftCorner = new Vector2(X,Y);
            SCALE = _scale;
            interactables = new InteractableManager();
        }

        public void SetData(Texture2D _layer2, Texture2D _layer1, string _id, string _name, TileMap.TileType _type) {
            tileData = new TileData(_layer2,_layer1,_id,_name,_type);
        }

        public Rectangle GetRectangle(Vector2 OFFSET) {
            return new Rectangle((int) upperLeftCorner.X + (int) OFFSET.X,(int) upperLeftCorner.Y + (int) OFFSET.Y,SCALE,SCALE);
        }

        public Texture2D GetTexture(int layer) {
            if (layer == 0) {
                return tileData.layer1Texture;
            } else {
                return tileData.layer2Texture;
            }
        }
    }

    public class TileMap {
        private Texture2D blank;
        private Texture2D sky;
        private Texture2D brick;
        private Tile[,] tileMap;
        private Tile[,] viewableTileMap;
        private int layer = 0;
        public int SCALE = 200;
        private Vector2 OFFSET = new Vector2(0,-15);

        public enum TileType {
            Special,
            DormRoom,
            Hallway,
            Stairs
        }

        public TileMap() {
            tileMap = new Tile[30,30];
            viewableTileMap = new Tile[15,7];
        }

        public void Initialize() {
            for(int i = 0; i < tileMap.GetUpperBound(0)+1; i++) {
                for(int j = 0; j < tileMap.GetUpperBound(1)+1; j++) {
                    tileMap[i,j] = new Tile(SCALE*i,SCALE*j, SCALE);
                }
            }
            for(int i = 0; i < viewableTileMap.GetUpperBound(0)+1; i++) {
                for(int j = 0; j < viewableTileMap.GetUpperBound(1)+1; j++) {
                    viewableTileMap[i,j] = new Tile(SCALE*i,SCALE*j + (int) OFFSET.Y, SCALE);
                }
            }
        }

        public void Update(GameTime gameTime, Player player) {
            layer = (int) player.playerPosition.Z;
            OFFSET = new Vector2(player.playerPosition.X,player.playerPosition.Y);
        }

        public void LoadContent(ContentManager Content) {
            blank = Content.Load<Texture2D>("tiles/blanktexture");
			sky = Content.Load<Texture2D>("sky");
			brick = Content.Load<Texture2D>("tiles/brick");
            Texture2D dorm1 = Content.Load<Texture2D>("tiles/insidedorms/dorm1");

            dynamic array;
            using (StreamReader file = File.OpenText("levels.json")) {
                string json = file.ReadToEnd();
                array = JsonConvert.DeserializeObject(json);
            }

                //for (int q = 0; q < array.Count; q++) {
            for(int i = 0; i < viewableTileMap.GetUpperBound(0); i++) {
                    Texture2D layer1 = Content.Load<Texture2D>("tiles/floor3/0/"+array[0][i].id);
                    Texture2D layer0 = dorm1;
                    TileMap.TileType _type = TileMap.TileType.DormRoom;

                    if (array[0][i].type == "Special") {
                        _type = TileMap.TileType.Special;
                    } else if (array[0][i].type == "DormRoom") {
                        _type = TileMap.TileType.DormRoom;
                    } else if (array[0][i].type == "Hallway") {
                        _type = TileMap.TileType.Hallway;
                    } else if (array[0][i].type == "Stairs") {
                        _type = TileMap.TileType.Stairs;
                    }

                    if (_type == TileMap.TileType.Special) {
                        layer0 = Content.Load<Texture2D>("tiles/floor3/-1/"+array[0][i].id);
                    }

                    for(int j = 0; j < viewableTileMap.GetUpperBound(1); j++) {
                        viewableTileMap[i,j].SetData(layer0, layer1,(string) array[0][i].id,(string) array[0][i].name, _type);
                    }
                }
            

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Player player1) {
			spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.None,null,null);
            if(layer <= -1) {
                for(int i = 0; i < viewableTileMap.GetUpperBound(0); i++) {
                    for(int j = 0; j < viewableTileMap.GetUpperBound(1); j++) {
                        //if(viewableTileMap[i,j].tileData.type == TileMap.TileType.Special &&
                        //    (viewableTileMap[i,j+1].tileData.type == TileMap.TileType.Special ||
                        //    viewableTileMap[i,j-1].tileData.type == TileMap.TileType.Special)) {
                        //        spriteBatch.Draw(sky, viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                        //        spriteBatch.Draw(viewableTileMap[i,j].GetTexture(layer), viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                        if (!viewableTileMap[i,j].GetRectangle(OFFSET).Intersects(player1.getCurrentRectangle(graphics))) {
                            spriteBatch.Draw(brick,viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                        } else {
                            spriteBatch.Draw(sky, viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                            spriteBatch.Draw(viewableTileMap[i,j].GetTexture(layer), viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                        }
                    }
                }
            } else {
                for(int i = 0; i < viewableTileMap.GetUpperBound(0); i++) {
                    for(int j = 0; j < viewableTileMap.GetUpperBound(1); j++) {
                        if (!(player1.getCurrentRectangle(graphics).Y < viewableTileMap[i,j].GetRectangle(OFFSET).Y + player1.getCurrentRectangle(graphics).Height) ||
                            !(player1.getCurrentRectangle(graphics).Y > viewableTileMap[i,j].GetRectangle(OFFSET).Y - player1.getCurrentRectangle(graphics).Height)) {
                            spriteBatch.Draw(brick, viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                        } else {
                            spriteBatch.Draw(sky, viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                            spriteBatch.Draw(viewableTileMap[i,j].GetTexture(layer-1), viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                            spriteBatch.Draw(viewableTileMap[i,j].GetTexture(layer), viewableTileMap[i,j].GetRectangle(OFFSET),Color.White);
                        }
                    }
                }
            }
            spriteBatch.End();
        }

    }
}