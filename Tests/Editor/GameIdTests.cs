using Gameframe.InfoTables;
using UnityEngine;
using NUnit.Framework;

public class GameIdTests
{

  [Test]
  public void GameId_TwoGameIdWithSameKeyAreEqual()
  {
    var gameIdOne = new InfoId("test");
    var gameIdTwo = new InfoId("test");
    Assert.AreEqual(gameIdOne.Value, gameIdTwo.Value);
    Assert.AreEqual(gameIdOne, gameIdTwo);
  }

  [Test]
  public void GameId_TwoGameIdWithDifferentKeyAreNotEqual()
  {
    var one = new InfoId("test1");
    var two = new InfoId("test2");
    Assert.AreNotEqual(one.Value, two.Value);
    Assert.AreNotEqual(one, two);
  }

  [Test]
  public void GameId_InvalidIsInvalid()
  {
    Assert.False(InfoId.Invalid.IsValid());
  }

  [Test]
  public void GameId_SerializesAndDeserializes()
  {
    var gameId = new InfoId("test");
    var json = JsonUtility.ToJson(gameId);
    var deserializedId = JsonUtility.FromJson<InfoId>(json);
    Assert.AreEqual(gameId.Value, deserializedId.Value);
    Assert.AreEqual(gameId, deserializedId);
  }

}
