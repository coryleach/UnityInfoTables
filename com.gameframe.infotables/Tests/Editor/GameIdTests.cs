using Gameframe.InfoTables;
using UnityEngine;
using NUnit.Framework;

public class GameIdTests
{

  [Test]
  public void GameId_TwoGameIdWithSameKeyAreEqual()
  {
    var gameIdOne = new GameId("test");
    var gameIdTwo = new GameId("test");
    Assert.AreEqual(gameIdOne.Value, gameIdTwo.Value);
    Assert.AreEqual(gameIdOne, gameIdTwo);
  }

  [Test]
  public void GameId_TwoGameIdWithDifferentKeyAreNotEqual()
  {
    var one = new GameId("test1");
    var two = new GameId("test2");
    Assert.AreNotEqual(one.Value, two.Value);
    Assert.AreNotEqual(one, two);
  }

  [Test]
  public void GameId_InvalidIsInvalid()
  {
    Assert.False(GameId.Invalid.IsValid());
  }

  [Test]
  public void GameId_SerializesAndDeserializes()
  {
    var gameId = new GameId("test");
    var json = JsonUtility.ToJson(gameId);
    var deserializedId = JsonUtility.FromJson<GameId>(json);
    Assert.AreEqual(gameId.Value, deserializedId.Value);
    Assert.AreEqual(gameId, deserializedId);
  }

}
