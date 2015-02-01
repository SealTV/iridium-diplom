namespace Assets.Scripts
{
    using Iridium.Utils.Data;

    public static class GlobalData
    {
        public static PacketsFromMaster.LevelData LevelData;

        static GlobalData()
        {
            //LevelData = new PacketsFromMaster.LevelData(1,1,"{name: \"Star Wars. Level 1\",game_id: 1,level_id: 1,input: {enemies: [{id: 1,start_position: {x: 10,y: 15},name: \"first\",speed: 2,health: 1}]},output: [1]}");
        }
    }
}