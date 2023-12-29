using Colegio;
using Colegio.Models;
using Colegio.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

public class EstudentTests
{
    [Fact]
    public void GetEstudents_ReturnsEstudentList_WhenDataExists()
    {
        // Arrange
        var data = new List<EstudentDto>
        {
            new EstudentDto {Id = Guid.NewGuid(),Name= "Prueba",Email="prueba@prueba.com" },
            new EstudentDto {Id = Guid.NewGuid(),Name= "Prueba2",Email="prueba2@prueba.com"}
        }.AsQueryable();

        Mock<DbSet<EstudentDto>> mockSet = MoqDbsetEstudent(data);

        var mockContext = new Mock<IColegioContext>();
        mockContext.Setup(c => c.Estudents).Returns(mockSet.Object);

        var mockLogger = new Mock<ILogger<Estudent>>();
        var service = new Estudent(mockLogger.Object, mockContext.Object);

        // Act
        var result = service.GetEstudents();

        // Assert
        Assert.Equal(data.ToList(), result);
    }

    private static Mock<DbSet<EstudentDto>> MoqDbsetEstudent(IQueryable<EstudentDto> data)
    {
        var mockSet = new Mock<DbSet<EstudentDto>>();
        mockSet.As<IQueryable<EstudentDto>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<EstudentDto>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<EstudentDto>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        mockSet.Setup(m => m.Add(It.IsAny<EstudentDto>())).Callback<EstudentDto>((s) => data.ToList().Add(s));
        mockSet.Setup(m => m.Remove(It.IsAny<EstudentDto>())).Callback<EstudentDto>((s) => data.ToList().Remove(s));
        mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => data.FirstOrDefault(d => d.Id == (Guid)ids[0]));
        return mockSet;
    }

    [Fact]
    public void GetEstudents_ReturnsNull()
    {
        // Arrange
        var data = new List<EstudentDto>();

        Mock<DbSet<EstudentDto>> mockSet = MoqDbsetEstudent(data.AsQueryable());

        var mockContext = new Mock<IColegioContext>();
        mockContext.Setup(c => c.Estudents).Returns(mockSet.Object);

        var mockLogger = new Mock<ILogger<Estudent>>();
        var service = new Estudent(mockLogger.Object, mockContext.Object);

        // Act
        var result = service.GetEstudents();

        // Assert
        Assert.Equal(data.ToList(), result);
    }

    [Fact]
    public void CreateEstudent_ReturnsTrue_WhenEstudentIsCreated()
    {
        // Arrange
        var estudent = new EstudentDto { Name = "Prueba", Email = "prueba@prueba.com" };

        var mockSet = new Mock<DbSet<EstudentDto>>();
        mockSet.Setup(m => m.Add(It.IsAny<EstudentDto>()));

        var mockContext = new Mock<IColegioContext>();
        mockContext.Setup(c => c.Estudents).Returns(mockSet.Object);

        var mockLogger = new Mock<ILogger<Estudent>>();
        var service = new Estudent(mockLogger.Object, mockContext.Object);

        // Act
        var result = service.CreateEstudent(estudent);

        // Assert
        Assert.True(result);
        mockSet.Verify(m => m.Add(It.IsAny<EstudentDto>()), Times.Once());
        mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }

    [Fact]
    public void DeleteEstudent_ReturnsTrue_WhenEstudentExists()
    {
        // Arrange
        var estudentId = Guid.NewGuid();
        var estudent = new EstudentDto { Id = estudentId, Name = "Test", Email = "test@test.com" };

        var data = new List<EstudentDto> { estudent }.AsQueryable();
        var mockSet = MoqDbsetEstudent(data);

        var mockContext = new Mock<IColegioContext>();
        mockContext.Setup(c => c.Estudents).Returns(mockSet.Object);

        var mockLogger = new Mock<ILogger<Estudent>>();
        var service = new Estudent(mockLogger.Object, mockContext.Object);

        // Act
        var result = service.DeleteEstudent(estudentId);

        // Assert
        Assert.True(result);
        mockSet.Verify(m => m.Remove(It.IsAny<EstudentDto>()), Times.Once());
        mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }

    [Fact]
    public void DeleteEstudent_ReturnsFalse_WhenEstudentDoesNotExist()
    {
        // Arrange
        var estudentId = Guid.NewGuid();

        var data = new List<EstudentDto>().AsQueryable();
        var mockSet = MoqDbsetEstudent(data);

        var mockContext = new Mock<IColegioContext>();
        mockContext.Setup(c => c.Estudents).Returns(mockSet.Object);

        var mockLogger = new Mock<ILogger<Estudent>>();
        var service = new Estudent(mockLogger.Object, mockContext.Object);

        // Act
        var result = service.DeleteEstudent(estudentId);

        // Assert
        Assert.False(result);
        mockSet.Verify(m => m.Remove(It.IsAny<EstudentDto>()), Times.Never());
        mockContext.Verify(m => m.SaveChanges(), Times.Never());
    }

    [Fact]
    public void DeleteEstudent_LogsError_WhenExceptionIsThrown()
    {
        // Arrange
        var estudentId = Guid.NewGuid();

        var mockSet = new Mock<DbSet<EstudentDto>>();
        mockSet.Setup(m => m.Find(estudentId)).Throws(new Exception("Test exception"));

        var mockContext = new Mock<IColegioContext>();
        mockContext.Setup(c => c.Estudents).Returns(mockSet.Object);

        var mockLogger = new Mock<ILogger<Estudent>>();
        var service = new Estudent(mockLogger.Object, mockContext.Object);

        // Act
        var result = service.DeleteEstudent(estudentId);

        // Assert
        Assert.False(result);
        mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
    }

    [Fact]
    public void GetEstudentByIdeentification_ReturnsEstudent_WhenEstudentExists()
    {
        // Arrange
        var estudentId = 123;
        var estudent = new EstudentDto { Id = Guid.NewGuid(), Name = "Prueba", Email = "prueba@prueba.com", Identification = estudentId };

        var data = new List<EstudentDto> { estudent }.AsQueryable();
        var mockSet = MoqDbsetEstudent(data);

        var mockContext = new Mock<IColegioContext>();
        mockContext.Setup(c => c.Estudents).Returns(mockSet.Object);

        var mockLogger = new Mock<ILogger<Estudent>>();
        var service = new Estudent(mockLogger.Object, mockContext.Object);

        // Act
        var result = service.GetEstudentByIdeentification(estudentId);

        // Assert
        Assert.Equal(estudent, result);
    }


}