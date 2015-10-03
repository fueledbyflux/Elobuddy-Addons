using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace RivenBuddy
{
    internal class WallJump
    {
        // you thought something would be here huh. niceme.me
        // soontm

        public static List<PositionStruct> Spots;

        static WallJump()
        {
            Spots = new List<PositionStruct>();
            if (Game.MapId == GameMapId.SummonersRift)
            {
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7830f, 4332f, 53.71266f), new Vector3(7760.892f, 4786.927f, 49.92004f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7902f, 4742f, 50.93093f), new Vector3(7787.4f, 4263.179f, 53.8678f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10850f, 4356f, -71.26904f), new Vector3(10383.91f, 4297.003f, -71.2406f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6696f, 8774f, -71.2406f), new Vector3(6933.048f, 8941.319f, 52.87134f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6698f, 8984f, 49.94371f), new Vector3(6462.293f, 8778.244f, -71.24048f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6830f, 8564f, -71.2406f), new Vector3(7180.054f, 8743.792f, 52.87256f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7096f, 8366f, -70.89347f), new Vector3(7354.568f, 8580.59f, 52.87244f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7634f, 6178f, 52.4513f), new Vector3(8115.281f, 6314.993f, -71.21191f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7542f, 6226f, 52.4513f), new Vector3(7800.893f, 6505.175f, -34.67603f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5096f, 11744f, 56.80341f), new Vector3(5224.156f, 12203.83f, 56.47681f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4776f, 11816f, 56.67248f), new Vector3(4843.92f, 12275.33f, 56.47705f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5498f, 11778f, 56.8484f), new Vector3(5627.228f, 12230.11f, 55.75854f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6351.196f, 11996.04f, 56.4768f), new Vector3(6265.39f, 11543.65f, 56.6145f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6374f, 11678f, 55.30333f), new Vector3(6343.67f, 12142.45f, 56.47705f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6598f, 11996f, 56.4768f), new Vector3(6689.555f, 11571.94f, 53.82983f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6652f, 11778f, 53.82969f), new Vector3(6593.762f, 12196.2f, 56.47681f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5574f, 12106f, 56.45419f), new Vector3(5548.412f, 11644.23f, 56.84839f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6696f, 11334f, 53.76143f), new Vector3(7009.122f, 11047.42f, 56.05029f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6974f, 10178f, 53.17089f), new Vector3(6948.026f, 10633.8f, 55.99829f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8210f, 3562f, 52.44194f), new Vector3(7961.107f, 3854.945f, 53.72083f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7974f, 3834f, 53.72089f), new Vector3(8194.845f, 3528.548f, 52.14685f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7572f, 8956f, 52.8726f), new Vector3(7790.026f, 9295.767f, 52.45325f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7372f, 9106f, 52.8726f), new Vector3(7708.071f, 9405.216f, 52.39282f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7018f, 10584f, 55.99712f), new Vector3(7070.215f, 10143.04f, 52.79089f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6224f, 12778f, 54.0266f), new Vector3(6175.879f, 13218.46f, 52.83826f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6359.42f, 13198.41f, 52.8381f), new Vector3(6519.4f, 12739.75f, 55.48572f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6982f, 11122f, 55.99969f), new Vector3(6572.649f, 11412.77f, 55.05566f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7750f, 10706f, 50.76583f), new Vector3(7365.367f, 10769.14f, 56.38928f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7394f, 10750f, 56.1977f), new Vector3(7761.647f, 10581.55f, 50.73694f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8194f, 11064f, 50.62045f), new Vector3(8604.129f, 11278.68f, 51.7439f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8574.423f, 11208.04f, 51.4786f), new Vector3(8130.391f, 11013.55f, 50.59875f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8094f, 9642f, 52.07279f), new Vector3(8479.859f, 9675.006f, 50.38391f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8375.989f, 9806.928f, 50.38285f), new Vector3(7974.133f, 9827.299f, 51.17188f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8646f, 9584f, 50.38403f), new Vector3(8654.872f, 9225.769f, 53.09827f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8670f, 9272f, 52.77951f), new Vector3(8490.986f, 9651.499f, 50.38428f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9400f, 12550f, 52.41484f), new Vector3(8931.289f, 12490.32f, 56.47681f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8998f, 12612f, 56.4768f), new Vector3(9476.425f, 12634.55f, 52.43921f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7122f, 12812f, 56.4768f), new Vector3(7200.856f, 13268.59f, 52.83826f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9772f, 12674f, 52.30659f), new Vector3(10198.09f, 12691.25f, 91.42993f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10140f, 12720f, 91.42984f), new Vector3(9682.941f, 12717.68f, 52.36975f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9844f, 12312f, 52.3063f), new Vector3(10244.92f, 12435.43f, 91.42981f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10104f, 12390f, 91.42984f), new Vector3(9662.812f, 12270.61f, 52.30627f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9946f, 11968f, 52.3063f), new Vector3(10319.01f, 12046.75f, 91.43005f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10200f, 11994f, 91.42978f), new Vector3(9760.642f, 11839.77f, 52.30627f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9996f, 11672f, 52.3063f), new Vector3(10351.38f, 11851.09f, 91.42957f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10322.26f, 11656.51f, 91.42979f), new Vector3(9941.854f, 11452.55f, 52.3064f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10890f, 12376f, 91.42981f), new Vector3(10894.18f, 12809.14f, 91.43018f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10888f, 12684f, 91.42981f), new Vector3(10887.33f, 12221.07f, 91.42969f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12294f, 10990f, 91.42981f), new Vector3(12645.71f, 10780.09f, 91.42993f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12476f, 10806.76f, 91.42981f), new Vector3(12294.53f, 11196.42f, 91.42969f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11494f, 10484f, 91.4298f), new Vector3(11384.13f, 10058.49f, 52.30688f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11428f, 10182f, 52.3063f), new Vector3(11524.18f, 10633.09f, 91.42969f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11865.53f, 10351f, 91.42981f), new Vector3(11722.06f, 9893.707f, 52.3064f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11776f, 10078f, 52.3063f), new Vector3(11908.76f, 10501.42f, 91.42944f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12230f, 10236f, 91.42981f), new Vector3(12192.78f, 9794.665f, 52.3064f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12126f, 9928f, 52.3063f), new Vector3(12222.6f, 10359.59f, 91.42969f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12568f, 10234f, 91.42981f), new Vector3(12515.01f, 9799.781f, 52.30615f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12524f, 9928f, 52.3063f), new Vector3(12565.06f, 10381.16f, 91.42969f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11268f, 7484f, 52.20285f), new Vector3(11212.31f, 7042.612f, 51.72363f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11422f, 7208f, 51.72648f), new Vector3(11520.11f, 7675.308f, 52.21631f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11552f, 7488f, 52.20296f), new Vector3(11446.06f, 7050.877f, 51.72339f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10726f, 7170f, 51.7226f), new Vector3(10299.64f, 7346.098f, 51.88916f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10394f, 7348f, 51.79094f), new Vector3(10804.31f, 7112.198f, 51.72241f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11256f, 11290f, 91.42986f), new Vector3(10945.58f, 10998.41f, 91.42981f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11044f, 11114f, 92.07098f), new Vector3(11341.25f, 11371.78f, 91.42969f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12941.35f, 10193.76f, 91.42978f), new Vector3(12858.71f, 9749.06f, 52.26074f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12822f, 9878f, 48.18407f), new Vector3(12855.1f, 10319.91f, 91.42993f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(13636f, 10420f, 91.42979f), new Vector3(13621.92f, 10859.09f, 91.42993f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(13668.66f, 10712.31f, 91.42981f), new Vector3(13624.57f, 10254.54f, 91.42969f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10610f, 13710f, 94.01597f), new Vector3(10211.83f, 13685.04f, 98.49829f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10344f, 13652f, 96.73087f), new Vector3(10689.69f, 13672.11f, 91.54248f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8873.54f, 10412.11f, 50.52374f), new Vector3(9075.934f, 9979.541f, 48.40625f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8194f, 10702f, 49.78528f), new Vector3(8674.658f, 10671.49f, 50.52466f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8550f, 10652f, 50.52413f), new Vector3(8074.135f, 10599.5f, 49.72729f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11776f, 8858f, 50.30732f), new Vector3(11408.74f, 8556.481f, 60.1958f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11744f, 8520f, 57.36847f), new Vector3(12054.6f, 8837.893f, 50.4707f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11744f, 8310f, 55.47791f), new Vector3(12135.24f, 8399.932f, 52.31201f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12076.03f, 8258.561f, 52.3117f), new Vector3(11633.12f, 8114.488f, 53.89453f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11024f, 8134f, 62.62095f), new Vector3(11155.47f, 7710.521f, 52.20581f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11100f, 7776f, 52.20348f), new Vector3(11015.81f, 8214.057f, 62.448f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10776f, 8408f, 63.09288f), new Vector3(10311.45f, 8501.852f, 63.427f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10484f, 8584f, 64.92075f), new Vector3(10891.32f, 8359.844f, 62.68604f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10918f, 7494f, 52.20335f), new Vector3(10936.67f, 7034.37f, 51.72266f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10972f, 7208f, 51.72375f), new Vector3(11016.62f, 7641.816f, 52.20361f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5874f, 9456f, -71.2406f), new Vector3(5947.653f, 9891.044f, 53.1123f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5922f, 9884f, 52.9804f), new Vector3(5828.5f, 9479.167f, -71.24048f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4032f, 2230f, 95.74808f), new Vector3(4105.018f, 2666.675f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4948f, 8386f, 25.79336f), new Vector3(4933.921f, 7908.062f, 52.02856f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3864f, 7420f, 51.61989f), new Vector3(3830.952f, 7881.322f, 51.96851f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3598f, 7416f, 51.89053f), new Vector3(3642.549f, 7868.615f, 53.93628f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4146f, 7736f, 50.63115f), new Vector3(4528.448f, 7545.06f, 51.10376f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3296f, 1140f, 95.74805f), new Vector3(3666.198f, 1427.618f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4148f, 1260f, 95.74805f), new Vector3(4478.822f, 1263.775f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4024f, 2534f, 95.74808f), new Vector3(3994.615f, 2128.424f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3098f, 3078f, 93.37599f), new Vector3(3414.418f, 3408.074f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2084f, 4664f, 95.74805f), new Vector3(2091.404f, 5181.122f, 53.39844f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3302f, 3404f, 95.74803f), new Vector3(3143.135f, 3002.351f, 95.67896f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3546f, 3582f, 95.74805f), new Vector3(3849.985f, 3909.208f, 95.74829f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3758f, 3788f, 95.74806f), new Vector3(3461.857f, 3466.945f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3652f, 1240f, 94.38851f), new Vector3(3213.636f, 1229.118f, 95.74829f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2508f, 3932f, 95.74786f), new Vector3(2264.065f, 4246.885f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2364f, 4144f, 95.74799f), new Vector3(2575.738f, 3820.933f, 95.7478f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2334f, 4724f, 95.74805f), new Vector3(2421.255f, 5186.035f, 52.89868f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2624f, 4658f, 95.74805f), new Vector3(2675.792f, 5112.661f, 53.17065f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2904f, 4630f, 95.74805f), new Vector3(3029.563f, 5032.221f, 53.52295f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3140f, 4524f, 95.74805f), new Vector3(3344.618f, 4941.21f, 54.14868f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3274f, 4834f, 54.14972f), new Vector3(3171.109f, 4389.878f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2960f, 4892f, 53.73007f), new Vector3(2843.646f, 4462.927f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2660f, 5010f, 53.07819f), new Vector3(2619.296f, 4545.331f, 95.74829f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2374f, 5058f, 52.56479f), new Vector3(2331.146f, 4545.331f, 95.74854f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2074f, 5034f, 52.54222f), new Vector3(2064.493f, 4532.074f, 95.74854f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4742f, 2048f, 95.74805f), new Vector3(5134.207f, 2065.609f, 52.13721f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4678f, 2192f, 95.74805f), new Vector3(5126.953f, 2271.042f, 51.52222f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5000f, 2188f, 51.87373f), new Vector3(4493.176f, 2145.012f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5050f, 2002f, 52.27983f), new Vector3(4623.25f, 1819.071f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5000f, 2552f, 51.26714f), new Vector3(4535.013f, 2430.725f, 95.74829f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4696f, 2472f, 95.74805f), new Vector3(5150.116f, 2643.843f, 51.25903f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4952f, 2898f, 51.08503f), new Vector3(4488.702f, 2719.693f, 95.74829f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4698f, 2772f, 95.74805f), new Vector3(5119.878f, 2965.672f, 51.09546f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(1174f, 3380f, 95.68596f), new Vector3(1151.922f, 3890.267f, 95.74829f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(1178f, 3794f, 95.66292f), new Vector3(1167.029f, 3327.453f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(1164f, 4128f, 95.74802f), new Vector3(1155.062f, 4555.145f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(1174f, 4434f, 95.74805f), new Vector3(1162.369f, 3998.082f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3012f, 6054f, 57.04691f), new Vector3(3339.819f, 6344.271f, 52.29932f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3160f, 6340f, 51.89334f), new Vector3(2844.186f, 5981.917f, 57.04443f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2798f, 6658f, 57.01933f), new Vector3(3249.252f, 6631.022f, 51.69019f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3052f, 6598f, 51.30767f), new Vector3(2611.247f, 6490.441f, 57.01782f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3592f, 7692f, 51.80717f), new Vector3(3515.772f, 7248.599f, 51.77148f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3840f, 7684f, 51.87151f), new Vector3(3840.06f, 7292.919f, 51.09668f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3682f, 7034f, 49.89319f), new Vector3(3727.482f, 6578.841f, 52.46167f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3642f, 6724f, 52.45851f), new Vector3(3636.869f, 7112.767f, 50.98193f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3344f, 7428f, 51.89142f), new Vector3(3330.87f, 7847.645f, 52.22314f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3324f, 7684f, 51.46717f), new Vector3(3279.267f, 7264.259f, 51.89063f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4462f, 7526f, 50.83388f), new Vector3(4041.504f, 7715.907f, 51.75513f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2244f, 7776f, 50.41083f), new Vector3(2533.404f, 8146.746f, 51.82056f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2516f, 8084f, 51.82605f), new Vector3(2246.906f, 7663.188f, 50.40967f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(1896f, 7974f, 51.16259f), new Vector3(2168.052f, 8336.56f, 51.7771f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2256f, 8184f, 51.78577f), new Vector3(1958.272f, 7851.851f, 50.64185f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(1698f, 8554f, 52.8381f), new Vector3(2136.605f, 8631.22f, 51.77734f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6324f, 3558f, 49.66662f), new Vector3(6719.724f, 3853.052f, 48.59741f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6654f, 3842f, 48.52352f), new Vector3(6249.627f, 3601.047f, 49.82275f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6274f, 4308f, 48.87802f), new Vector3(6708.84f, 4205.648f, 48.52539f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6650f, 4286f, 48.52583f), new Vector3(6210.893f, 4448.303f, 48.53638f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5958f, 4632f, 48.53367f), new Vector3(5638.711f, 4969.978f, 48.81689f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5748f, 4942f, 48.31442f), new Vector3(6085.081f, 4624.43f, 48.53369f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6180f, 5326f, 48.52729f), new Vector3(6042.292f, 5622.549f, 51.7804f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6130f, 5584f, 51.7751f), new Vector3(6215.104f, 5177.999f, 48.52795f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6444f, 5176f, 48.52723f), new Vector3(6892.5f, 5190.152f, 48.52698f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(6752f, 5102f, 48.527f), new Vector3(6281.794f, 5080.849f, 48.5282f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7198f, 5542f, 55.9738f), new Vector3(7521.103f, 5873.835f, 52.59497f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7324f, 5908f, 52.50191f), new Vector3(7108.314f, 5540.416f, 56.42932f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8568f, 3236f, 54.356f), new Vector3(8527.594f, 2740.154f, 50.6095f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8254f, 2880f, 51.13f), new Vector3(8156.763f, 3340.199f, 51.56714f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8476f, 2888f, 51.13f), new Vector3(8493.787f, 3355.073f, 54.08716f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9318f, 3136f, 55.41505f), new Vector3(9241.075f, 2710.113f, 49.22266f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9424f, 2832f, 49.22298f), new Vector3(9472.017f, 3274.394f, 54.85278f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10204f, 2184f, 49.2229f), new Vector3(10182.22f, 1718.163f, 50.06506f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9772f, 3036f, 63.28237f), new Vector3(9691.301f, 2617.71f, 49.2229f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9924f, 2732f, 49.22295f), new Vector3(9915.359f, 3167.894f, 55.28052f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9004f, 1988f, 64.91981f), new Vector3(8720.87f, 1611.468f, 49.45776f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8792f, 1732f, 49.4511f), new Vector3(9017.521f, 2151.304f, 54.97693f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8522f, 1730f, 49.4503f), new Vector3(8498.881f, 2160.272f, 51.12988f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8364f, 2036f, 51.13f), new Vector3(8331.094f, 1575.509f, 49.45837f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7574f, 1732f, 49.44834f), new Vector3(7661.29f, 2154.325f, 51.15857f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7672f, 2108f, 51.1551f), new Vector3(7637.703f, 1644.864f, 49.44763f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8822f, 5058f, 51.75765f), new Vector3(8871.964f, 5619.657f, -71.24048f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8928f, 5434f, -71.2406f), new Vector3(8770.28f, 4940.507f, 51.90063f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9090f, 4758f, 51.56561f), new Vector3(9508.204f, 4649.149f, -71.24023f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9370f, 4682f, -71.2406f), new Vector3(8946.168f, 4803.055f, 51.78931f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9044f, 4406f, 52.701f), new Vector3(9466.732f, 4418.132f, -71.23999f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9302f, 4456f, -71.2406f), new Vector3(8844.863f, 4401.378f, 53.06079f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9486f, 4134f, -68.90685f), new Vector3(9167.848f, 3772.066f, 55.40771f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9154f, 3916f, 54.35764f), new Vector3(9588.933f, 4202.336f, -71.24023f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9704f, 3886f, -71.2406f), new Vector3(9524.099f, 3456.417f, 60.53613f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9522f, 3558f, 64.14748f), new Vector3(9767.617f, 4060.979f, -71.24097f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9912f, 3834f, -71.2406f), new Vector3(9675.586f, 3375.487f, 52.74512f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10650f, 4506f, -72.7732f), new Vector3(10260.96f, 4422.892f, -71.24072f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10488f, 4356f, -71.2406f), new Vector3(10924.26f, 4368.185f, -71.22656f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9724f, 5184f, -72.09716f), new Vector3(9741.12f, 4738.446f, -71.24097f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(9766f, 4928f, -71.2406f), new Vector3(9731.139f, 5331.521f, -68.29907f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7976f, 5930f, 50.13454f), new Vector3(7850.892f, 6385.171f, -60.94141f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8002f, 6168f, -71.67461f), new Vector3(7561.733f, 6066.61f, 52.45313f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8078f, 6138f, -71.2406f), new Vector3(7976.015f, 5761.078f, 51.7417f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7372f, 6382f, 52.4513f), new Vector3(7682.28f, 6651.342f, 46.63013f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(7944f, 5966f, 50.25438f), new Vector3(8413.663f, 6142.276f, -71.24048f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4578f, 5848f, 51.72738f), new Vector3(4661.317f, 5510.839f, 50.23816f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4712f, 5630f, 50.2312f), new Vector3(4441.707f, 5890.851f, 52.42834f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4042f, 6420f, 52.46645f), new Vector3(4407.121f, 6251.123f, 51.33948f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4810f, 3288f, 50.86842f), new Vector3(4346.738f, 3092.491f, 95.74805f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4524f, 3258f, 95.74808f), new Vector3(4858.832f, 3542.613f, 50.75488f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4336f, 6278f, 51.34194f), new Vector3(3927.237f, 6465.769f, 52.46436f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5850f, 2462f, 52.13899f), new Vector3(5379.39f, 2412.754f, 51.24487f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5444f, 2326f, 51.1855f), new Vector3(5894.803f, 2372.421f, 52.1394f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(8162f, 3136f, 51.5508f), new Vector3(8279.901f, 2703.895f, 51.12988f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2002f, 8654f, 51.77731f), new Vector3(1546.777f, 8653.186f, 52.83862f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2870f, 10322f, 54.3255f), new Vector3(3129.455f, 10764.55f, -67.69873f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3106f, 10584f, -70.85432f), new Vector3(2784.672f, 10134.64f, 54.32544f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3304f, 10136f, -64.47968f), new Vector3(2940.083f, 9778.102f, 52.79517f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3128f, 9976f, 53.15556f), new Vector3(3452.468f, 10388.89f, -66.53906f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3246f, 9614f, 50.8065f), new Vector3(3711.912f, 9774.602f, -68.27612f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(3500f, 9744f, -59.57598f), new Vector3(3043.061f, 9559.309f, 51.10278f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4112f, 8022f, 50.71408f), new Vector3(4482.876f, 8252.238f, 48.91431f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(4416.438f, 8110.586f, 48.79285f), new Vector3(3986.606f, 7906.659f, 51.28076f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2596f, 9328f, 51.77343f), new Vector3(2984.94f, 9366.622f, 50.72021f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2802f, 9392f, 50.85761f), new Vector3(2379.708f, 9141.052f, 51.77612f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(2202f, 9216f, 51.77626f), new Vector3(1791.216f, 9519.231f, 52.83813f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(1844f, 9384f, 52.8381f), new Vector3(2188.243f, 9065.282f, 51.77637f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10418f, 1782f, 49.35614f), new Vector3(10381.67f, 2239.321f, 49.2229f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5488f, 10350f, -71.18363f), new Vector3(5886.312f, 10259.44f, 54.63257f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5800f, 10358f, 54.52341f), new Vector3(5425.606f, 10435.02f, -71.24048f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5756f, 10678f, 55.75924f), new Vector3(5349.564f, 10593.63f, -71.24072f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5496f, 10640f, -71.2406f), new Vector3(5803.17f, 10843.28f, 55.97473f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5348f, 10822f, -71.2406f), new Vector3(5631.257f, 11058.52f, 56.8324f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5578f, 11046f, 56.8512f), new Vector3(5270.925f, 10779.2f, -71.24048f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(5134f, 10978f, -71.23817f), new Vector3(5300.562f, 11291.07f, 56.82715f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10700f, 6948f, 51.7226f), new Vector3(10270.1f, 6769.453f, 51.99146f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(10394f, 6822f, 51.97513f), new Vector3(10832.54f, 6996.207f, 51.72241f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12794f, 6256f, 51.66834f), new Vector3(13234.44f, 6234.645f, 55.27124f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(13178.31f, 6107.714f, 55.41211f), new Vector3(12688.38f, 6129.937f, 54.74561f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12734f, 6580f, 51.66419f), new Vector3(13014.84f, 6909.749f, 52.10498f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12925.02f, 6908.355f, 51.85595f), new Vector3(12586.91f, 6516.82f, 51.72314f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11344f, 5304f, -56.59019f), new Vector3(11707.61f, 5211.494f, 52.29712f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12744f, 5910f, 51.59283f), new Vector3(13180.69f, 5833.892f, 55.12866f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11744f, 4644f, -71.2406f), new Vector3(12140.57f, 4757.023f, 51.72949f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11978.53f, 4708.429f, 51.7294f), new Vector3(11508.46f, 4636.023f, -71.24048f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11994f, 5696f, 52.02821f), new Vector3(12447.71f, 5707.316f, 53.01465f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12300f, 5820f, 58.94732f), new Vector3(11874.41f, 5715.601f, 51.16064f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11570f, 5224f, 51.75803f), new Vector3(11224.61f, 5177.344f, -70.22119f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(12050f, 4540f, 51.86011f), new Vector3(11763.1f, 4227.405f, -71.24072f)));
                Spots.Add(
                    new PositionStruct(
                        new Vector3(11894f, 4348f, -71.2406f), new Vector3(12161.24f, 4637.81f, 51.72925f)));
            }
        }

        public static PositionStruct GetNearest(Vector3 pos)
        {
            foreach (var spot in Spots.OrderBy(p => p.Start.Distance(pos)))
            {
                var steps = (int) (spot.Start.Distance(ObjectManager.Player.ServerPosition)/10);
                var direction = (spot.Start - ObjectManager.Player.ServerPosition).Normalized();
                var found = true;
                for (var i = 0; i < steps - 1; i++)
                {
                    if (
                        NavMesh.GetCollisionFlags(ObjectManager.Player.ServerPosition + direction*10*i)
                            .HasFlag(CollisionFlags.Wall))
                    {
                        found = false;
                        break;
                    }
                }
                ;
                if (found)
                    return spot;
            }

            return PositionStruct.Zero;
        }

        public static PositionStruct GetSpot(Vector3 pos)
        {
            var s = Spots.Where(p => p.Start.Distance(pos) <= 10).OrderBy(q => q.Start.Distance(pos));
            if (s.Any())
                return s.FirstOrDefault();
            return PositionStruct.Zero;
        }

        public static bool DoWallJump()
        {
            if (SpellManager.Spells[SpellSlot.Q].IsReady() && SpellEvents.QCount == 2)
            {
                var spot = GetSpot(Player.Instance.Position);
                if (spot.Start.IsValid() && Player.Instance.Position.Distance(spot.Start) < 100)
                {
                    if (SpellManager.Spells[SpellSlot.E].IsReady())
                    {
                        Player.CastSpell(SpellSlot.E, spot.End);
                        Player.CastSpell(SpellSlot.Q, spot.End);
                        return true;
                    }
                }
                else
                {
                    var spot2 = GetNearest(Player.Instance.Position);
                    if (spot2.Start.IsValid() && spot2.Start.Distance(Player.Instance.Position) < 300)
                    {
                        Player.IssueOrder(GameObjectOrder.MoveTo, spot2.Start);
                        return true;
                    }
                }
            }
            return false;
        }

        public struct PositionStruct
        {
            public static PositionStruct Zero = new PositionStruct(Vector3.Zero, Vector3.Zero);
            public readonly Vector3 End;
            public readonly Vector3 Start;

            public PositionStruct(Vector3 start, Vector3 end)
            {
                Start = start;
                End = end;
            }
        }
    }
}