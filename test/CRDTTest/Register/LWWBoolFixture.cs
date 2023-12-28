using CRDT.Register;

[TestFixture]
public class LWWBoolFixture
{
    [Test]
    public void CreateNewInstanceOfLWWBool()
    {
        // Arrange
        var timestamp = DateTime.UtcNow.Ticks;
        var lwwBool = new LWWBool(true);

        // Assert
        Assert.That(lwwBool.Value, Is.True);
        Assert.That(lwwBool.Timestamp, Is.GreaterThan(timestamp));
    }

    [Test]
    public void UpdateValueOfTheLWWBoolToTheSameAsBefore()
    {
        // Arrange
        var lwwBool = new LWWBool(true);
        var timestamp = lwwBool.Timestamp;

        // Act
        lwwBool.SetValue(true);

        // Assert
        Assert.That(lwwBool.Value, Is.True);
        Assert.That(lwwBool.Timestamp, Is.GreaterThan(timestamp));
    }

    [Test]
    public void ChangeValueOfTheLWWBool()
    {
        // Arrange
        var lwwBool = new LWWBool(true);
        var timestamp = lwwBool.Timestamp;

        // Act
        lwwBool.SetValue(false);

        // Assert
        Assert.That(lwwBool.Value, Is.False);
        Assert.That(lwwBool.Timestamp, Is.GreaterThan(timestamp));
    }

    [Test]
    public void MergeShouldNotChangeTheCurrentStateIfItsTheLatest()
    {
        // Arrange
        var replicaTwo = new LWWBool(false);
        var replicaOne = new LWWBool(true);
        var timestamp = replicaOne.Timestamp;

        // Act
        var mergedReplica = replicaOne ^ replicaTwo;

        // Assert
        Assert.That(mergedReplica.Value, Is.True);
        Assert.That(mergedReplica.Timestamp, Is.EqualTo(timestamp));
    }

    [Test]
    public void MergeShouldUpdateTheStateWhenTheOtherReplicaIsNewer()
    {
        // Arrange
        var replicaOne = new LWWBool(true);
        var replicaTwo = new LWWBool(false);
        var timestamp = replicaTwo.Timestamp;

        // Act
        var mergedReplica = replicaOne ^ replicaTwo;

        // Assert
        Assert.That(mergedReplica.Value, Is.False);
        Assert.That(mergedReplica.Timestamp, Is.EqualTo(timestamp));
    }

    [Test]
    public void MergeShouldUpdateTheStateWhenTheOtherReplicaIsNewerEvenIfValueIsTheSame()
    {
        // Arrange
        var replicaOne = new LWWBool(true);
        var replicaTwo = new LWWBool(true);
        var timestamp = replicaTwo.Timestamp;

        // Act
        var mergedReplica = replicaOne ^ replicaTwo;

        // Assert
        Assert.That(mergedReplica.Value, Is.True);
        Assert.That(mergedReplica.Timestamp, Is.EqualTo(timestamp));
    }

    [Test]
    public void MergeShouldBeCommutative()
    {
        // Arrange
        var replicaOne = new LWWBool(true);
        var replicaTwo = new LWWBool(false);

        // Act
        var leftSide = replicaOne ^ replicaTwo;
        var rightSide = replicaTwo ^ replicaOne;

        // Assert
        Assert.That(leftSide, Is.EqualTo(rightSide));
    }

    [Test]
    public void MergeShouldBeIdempotent()
    {
        // Arrange
        var replicaOne = new LWWBool(true);
        var timestamp = replicaOne.Timestamp;

        // Act
        var mergedReplica = replicaOne ^ replicaOne;

        // Assert
        Assert.That(mergedReplica.Value, Is.True);
        Assert.That(mergedReplica.Timestamp, Is.EqualTo(timestamp));
    }

    [Test]
    public void MergeShouldBeAssociative()
    {
        // Arrange
        var replicaOne = new LWWBool(true);
        var replicaTwo = new LWWBool(false);
        var replicaThree = new LWWBool(true);

        // Act
        var leftSide = replicaOne ^ replicaTwo ^ replicaThree;

        var rightSide = replicaThree ^ replicaOne ^ replicaTwo;

        // Assert
        Assert.That(leftSide, Is.EqualTo(rightSide));
    }
}