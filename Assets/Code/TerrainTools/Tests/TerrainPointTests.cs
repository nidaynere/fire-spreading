
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace TerrainTools.Tests {
    public class TerrainPointTests {
        [UnityTest]
        public IEnumerator ConversionTest () {
            var terrainPointFinder = new TerrainPointFinder();

            var index = 2;
            var sizeX = 2;
            /*  
                 X       X
                 *X      X
                 X       X
            */
            var pos = terrainPointFinder.IndexToPosition(index, Vector3.zero, sizeX);

            Assert.AreEqual (0, (int)pos.x);
            Assert.AreEqual (1, (int)pos.z);

            var backToIndex = terrainPointFinder.PositionToIndex(pos, Vector3.zero, sizeX);
            Assert.AreEqual(index, backToIndex);


            // another
            index = 6;
            sizeX = 4;
            /*      
                 X       X      X       X
                 X       X      X*      X
                 X       X      X       X
            */
            pos = terrainPointFinder.IndexToPosition(index, Vector3.zero, sizeX);

            Assert.AreEqual(2, (int)pos.x);
            Assert.AreEqual(1, (int)pos.z);

            backToIndex = terrainPointFinder.PositionToIndex(pos, Vector3.zero, sizeX);
            Assert.AreEqual(index, backToIndex);

            yield break;
        }
    }
}
