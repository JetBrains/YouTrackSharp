﻿using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.AgileBoards;
using YouTrackSharp.Tests.Infrastructure;
#pragma warning disable 618

namespace YouTrackSharp.Tests.Integration.AgileBoards
{
    [UsedImplicitly]
    public partial class AgileBoardServiceTests
    {
        public class UpdateAgileBoard
        {
            [Fact(Skip = "YouTrack 2018.2 and higher no longer support the agile board API. Please check https://github.com/JetBrains/YouTrackSharp/issues/81 for more information.")]
            public async Task Valid_Connection_Updates_Agile_Board()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateAgileBoardService();

                var existingAgileBoards = await service.GetAgileBoards();
                foreach (var agileBoard in existingAgileBoards.Where(b => b.Projects.Any(p => p.Id == "DP1")))
                {
                    var updatedBoardName = DemoBoardNamePrefix + Guid.NewGuid();

                    // Act
                    await service.UpdateAgileBoard(agileBoard.Id, new AgileSettings
                    {
                        Name = updatedBoardName
                    });

                    // Assert
                    var updatedBoard = await service.GetAgileBoard(agileBoard.Id);
                    Assert.NotNull(updatedBoard.Id);
                    Assert.NotNull(updatedBoard.Name);
                    Assert.NotNull(updatedBoard.Projects);
                    Assert.NotNull(updatedBoard.ColumnSettings);
                    Assert.NotNull(updatedBoard.ColumnSettings.Field);
                    Assert.Equal(updatedBoardName, updatedBoard.Name);
                    Assert.Equal("State", updatedBoard.ColumnSettings.Field.Name);
                    Assert.Contains(updatedBoard.Projects, p => p.Id == "DP1");
                }
            }
        }
    }
}
