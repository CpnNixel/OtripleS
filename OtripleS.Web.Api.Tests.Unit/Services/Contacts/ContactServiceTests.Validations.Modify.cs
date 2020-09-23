﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Contacts
{
	public partial class ContactServiceTests
    {
		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenContactIsNullAndLogItAsync()
		{
			//given
			Contact invalidContact = null;
			var nullContactException = new NullContactException();

			var expectedContactValidationException =
				new ContactValidationException(nullContactException);

			//when
			ValueTask<Contact> modifyContactTask =
				this.contactService.ModifyContactAsync(invalidContact);

			//then
			await Assert.ThrowsAsync<ContactValidationException>(() =>
				modifyContactTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenContactIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidContactId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Contact invalidContact = randomContact;
            invalidContact.Id = invalidContactId;

            var invalidContactException = new InvalidContactException(
                parameterName: nameof(Contact.Id),
                parameterValue: invalidContact.Id);

            var expectedContactValidationException =
                new ContactValidationException(invalidContactException);

            //when
            ValueTask<Contact> modifyContactTask =
                this.contactService.ModifyContactAsync(invalidContact);

            //then
            await Assert.ThrowsAsync<ContactValidationException>(() =>
                modifyContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
