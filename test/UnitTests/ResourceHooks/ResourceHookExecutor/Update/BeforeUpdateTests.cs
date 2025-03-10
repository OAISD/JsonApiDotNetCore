﻿using JsonApiDotNetCore.Hooks;
using JsonApiDotNetCoreExample.Models;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.ResourceHooks
{
    public class BeforeUpdateTests : HooksTestsSetup
    {
        private readonly ResourceHook[] targetHooks = { ResourceHook.BeforeUpdate, ResourceHook.BeforeUpdateRelationship };

        [Fact]
        public void BeforeUpdate()
        {
            // arrange
            var todoDiscovery = SetDiscoverableHooks<TodoItem>(targetHooks, DisableDbValues);
            var personDiscovery = SetDiscoverableHooks<Person>(targetHooks, DisableDbValues);
            (var contextMock, var hookExecutor, var todoResourceMock,
                var ownerResourceMock) = CreateTestObjects(todoDiscovery, personDiscovery);
            var todoList = CreateTodoWithOwner();

            // act
            hookExecutor.BeforeUpdate(todoList, ResourcePipeline.Patch);

            // assert
            todoResourceMock.Verify(rd => rd.BeforeUpdate(It.IsAny<IEntityDiff<TodoItem>>(), ResourcePipeline.Patch), Times.Once());
            ownerResourceMock.Verify(rd => rd.BeforeUpdateRelationship(It.IsAny<HashSet<string>>(), It.IsAny<IRelationshipsDictionary<Person>>(), ResourcePipeline.Patch), Times.Once());
            VerifyNoOtherCalls(todoResourceMock, ownerResourceMock);
        }

        [Fact]
        public void BeforeUpdate_Without_Parent_Hook_Implemented()
        {
            // arrange
            var todoDiscovery = SetDiscoverableHooks<TodoItem>(NoHooks, DisableDbValues);
            var personDiscovery = SetDiscoverableHooks<Person>(targetHooks, DisableDbValues);
            (var contextMock, var hookExecutor, var todoResourceMock,
                var ownerResourceMock) = CreateTestObjects(todoDiscovery, personDiscovery);
            var todoList = CreateTodoWithOwner();

            // act
            hookExecutor.BeforeUpdate(todoList, ResourcePipeline.Patch);

            // assert
            ownerResourceMock.Verify(rd => rd.BeforeUpdateRelationship(It.IsAny<HashSet<string>>(), It.IsAny<IRelationshipsDictionary<Person>>(), ResourcePipeline.Patch), Times.Once());
            VerifyNoOtherCalls(todoResourceMock, ownerResourceMock);
        }

        [Fact]
        public void BeforeUpdate_Without_Child_Hook_Implemented()
        {
            // arrange
            var todoDiscovery = SetDiscoverableHooks<TodoItem>(targetHooks, DisableDbValues);
            var personDiscovery = SetDiscoverableHooks<Person>(NoHooks, DisableDbValues);

            (var contextMock, var hookExecutor, var todoResourceMock,
                var ownerResourceMock) = CreateTestObjects(todoDiscovery, personDiscovery);
            var todoList = CreateTodoWithOwner();

            // act
            hookExecutor.BeforeUpdate(todoList, ResourcePipeline.Patch);

            // assert
            todoResourceMock.Verify(rd => rd.BeforeUpdate(It.IsAny<IEntityDiff<TodoItem>>(), ResourcePipeline.Patch), Times.Once());
            VerifyNoOtherCalls(todoResourceMock, ownerResourceMock);
        }

        [Fact]
        public void BeforeUpdate_Without_Any_Hook_Implemented()
        {
            // arrange
            var todoDiscovery = SetDiscoverableHooks<TodoItem>(NoHooks, DisableDbValues);
            var personDiscovery = SetDiscoverableHooks<Person>(NoHooks, DisableDbValues);
            (var contextMock, var hookExecutor, var todoResourceMock,
                var ownerResourceMock) = CreateTestObjects(todoDiscovery, personDiscovery);
            var todoList = CreateTodoWithOwner();

            // act
            hookExecutor.BeforeUpdate(todoList, ResourcePipeline.Patch);

            // assert
            VerifyNoOtherCalls(todoResourceMock, ownerResourceMock);
        }
    }
}

