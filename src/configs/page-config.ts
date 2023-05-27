export const defaultConfig = {
  roles: {
    tableHeaders: {
      name: {
        colName: "name",
        flex: 10,
      },
      claims: {
        colName: "claims",
        flex: 10,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      name: {
        fieldName: "name",
        keyName: "name",
        component: "TextField",
      },
      claims: {
        fieldName: "claims",
        keyName: "claims",
        component: "Select",
        getter: "permissions",
      },
    },
  },
  permissions: {
    tableHeaders: {
      type: {
        colName: "type",
        flex: 10,
      },
      value: {
        colName: "value",
        flex: 10,
      },
    },
  },
  users: {
    tableHeaders: {
      userName: {
        colName: "userName",
        flex: 10,
      },
      email: {
        colName: "email",
        flex: 10,
      },
    },
  },
  voterProfileRevisions: {
    tableHeaders: {
      rsaPersonId: {
        colName: "rsaPersonId",
        flex: 2,
      },
      idnp: {
        colName: "idnp",
        flex: 2,
      },
      registrationDate: {
        colName: "registrationDate",
        flex: 2,
        formatted: true,
      },
      email: {
        colName: "email",
        flex: 2,
      },
      lastName: {
        colName: "lastName",
        flex: 2,
      },
      firstName: {
        colName: "firstName",
        flex: 2,
      },
      middleName: {
        colName: "middleName",
        flex: 2,
      },
      dateOfBirth: {
        colName: "dateOfBirth",
        flex: 2,
        formatted: true,
      },
      genderName: {
        colName: "genderName",
        flex: 2,
      },
      identityNumber: {
        colName: "identityNumber",
        flex: 2,
      },
      identitySeries: {
        colName: "identitySeries",
        flex: 2,
      },
      personStatusName: {
        colName: "personStatusName",
        flex: 2,
      },
      revision: {
        colName: "revision",
        flex: 2,
      },
      disactivationDate: {
        colName: "disactivationDate",
        flex: 2,
        formatted: true,
      },
      regionName: {
        colName: "regionName",
        flex: 2,
      },
      localityName: {
        colName: "localityName",
        flex: 2,
      },
      street: {
        colName: "street",
        flex: 2,
      },
      house: {
        colName: "house",
        flex: 2,
      },
      bloc: {
        colName: "bloc",
        flex: 2,
      },
      apartment: {
        colName: "apartment",
        flex: 2,
      },
    },
  },
  voterProfileAddress: {
    tableHeaders: {
      idnp: {
        colName: "idnp",
        flex: 2,
      },
      revision: {
        colName: "revision",
        flex: 2,
      },
      regionName: {
        colName: "regionName",
        flex: 2,
      },
      regionTypeName: {
        colName: "regionTypeName",
        flex: 2,
      },
      localityName: {
        colName: "localityName",
        flex: 2,
      },
      localityRegionTypeName: {
        colName: "localityRegionTypeName",
        flex: 3,
      },
      deactivationDate: {
        colName: "deactivationDate",
        flex: 2,
        formatted: true,
      },
      street: {
        colName: "street",
        flex: 2,
        isTooltip: true,
      },
      house: {
        colName: "house",
        flex: 2,
      },
      bloc: {
        colName: "bloc",
        flex: 2,
      },
      apartment: {
        colName: "apartment",
        flex: 2,
      },
    },
  },
  voterProfilePerson: {
    tableHeaders: {
      idnp: {
        colName: "idnp",
        width: 132,
      },
      dateOfBirth: {
        colName: "dateOfBirth",
        width: 106,
        formatted: true,
      },
      disactivationDate: {
        colName: "disactivationDate",
        width: 122,
        formatted: true,
      },
      email: {
        colName: "email",
        width: 136,
        isTooltip: true,
      },
      firstName: {
        colName: "firstName",
        width: 131,
      },
      genderName: {
        colName: "genderName",
        width: 100,
      },
      identityNumber: {
        colName: "identityNumber",
        width: 162,
      },
      identitySeries: {
        colName: "identitySeries",
        width: 100,
      },
      lastName: {
        colName: "lastName",
        width: 112,
      },
      middleName: {
        colName: "middleName",
        width: 120,
      },
      personStatusName: {
        colName: "personStatusName",
        width: 122,
      },
      registrationDate: {
        colName: "registrationDate",
        width: 130,
        formatted: true,
      },
      revision: {
        colName: "revision",
        width: 105,
      },
      rsaPersonId: {
        colName: "rsaPersonId",
        width: 140,
      },
      isAction: {
        colName: "isAction",
        width: 76,
      },
    },
  },
  voterProfilePersonId: {
    tableHeaders: {
      idnp: {
        colName: "idnp",
        width: 132,
      },
      dateOfBirth: {
        colName: "dateOfBirth",
        width: 106,
        formatted: true,
      },
      disactivationDate: {
        colName: "disactivationDate",
        width: 122,
        formatted: true,
      },
      email: {
        colName: "email",
        width: 136,
        isTooltip: true,
      },
      firstName: {
        colName: "firstName",
        width: 131,
      },
      genderName: {
        colName: "genderName",
        width: 100,
      },
      identityNumber: {
        colName: "identityNumber",
        width: 162,
      },
      identitySeries: {
        colName: "identitySeries",
        width: 100,
      },
      lastName: {
        colName: "lastName",
        width: 112,
      },
      middleName: {
        colName: "middleName",
        width: 120,
      },
      personStatusName: {
        colName: "personStatusName",
        width: 122,
      },
      registrationDate: {
        colName: "registrationDate",
        width: 163,
        formatted: true,
      },
      revision: {
        colName: "revision",
        width: 105,
      },
      rsaPersonId: {
        colName: "rsaPersonId",
        width: 182,
      },
    },
  },
  electionFunction: {
    tableHeaders: {
      authorFirstName: {
        colName: "authorFirstName",
        width: 148,
      },
      authorLastName: {
        colName: "authorLastName",
        width: 159,
      },
      createdDate: {
        colName: "createdDate",
        width: 126,
        formatted: true,
      },
      deleted: {
        colName: "deleted",
        width: 92,
      },
      deletedDate: {
        colName: "deletedDate",
        width: 109,
        formatted: true,
      },
      deletionAuthorFirstName: {
        colName: "deletionAuthorFirstName",
        width: 158,
      },
      deletionAuthorLastName: {
        colName: "deletionAuthorLastName",
        width: 170,
      },
      isElective: {
        colName: "isElective",
        width: 83,
      },
      lastModifiedDate: {
        colName: "lastModifiedDate",
        width: 133,
        formatted: true,
      },
      modificationAuthorFirstName: {
        colName: "modificationAuthorFirstName",
        width: 196,
        isTooltip: true,
      },
      modificationAuthorLastName: {
        colName: "modificationAuthorLastName",
        width: 191,
        isTooltip: true,
      },
      nameRo: {
        colName: "nameRo",
        width: 153,
        isTooltip: true,
      },
      isAction: {
        colName: "isAction",
      },
    },
    formFields: {
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
      },
      isElective: {
        fieldName: "isElective",
        keyName: "isElective",
        component: "Radio",
      },
    },
  },
  regions: {
    tableHeaders: {
      nameRo: {
        colName: "nameRo",
        flex: 10,
      },
      statisticCode: {
        colName: "statisticCode",
        flex: 10,
      },
      validFrom: {
        colName: "validFrom",
        flex: 10,
        formatted: true,
      },
      validTo: {
        colName: "validTo",
        flex: 10,
        formatted: true,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
      },
      statisticCode: {
        fieldName: "statisticCode",
        keyName: "statisticCode",
        component: "TextField",
        style: {
          gridColumn: "span 2",
        },
      },
      validFrom: {
        fieldName: "validFrom",
        keyName: "validFrom",
        component: "Date",
      },
      validTo: {
        fieldName: "validTo",
        keyName: "validTo",
        component: "Date",
      },
      saRegionId: {
        fieldName: "saRegionId",
        keyName: "saRegionId",
      },
      regionTypeId: {
        fieldName: "regionTypeId",
        keyName: "regionTypeId",
      },
    },
  },
  regionTypes: {
    tableHeaders: {
      nameRo: {
        colName: "nameRo",
        flex: 10,
      },
      descriptionRo: {
        colName: "descriptionRo",
        flex: 10,
      },
      councilRo: {
        colName: "councilRo",
        flex: 10,
      },
      mayorRo: {
        colName: "mayorRo",
        flex: 10,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
      },
      descriptionRo: {
        fieldName: "descriptionRo",
        keyName: "descriptionRo",
        component: "TextField",
      },
      descriptionRu: {
        fieldName: "descriptionRu",
        keyName: "descriptionRu",
        component: "TextField",
      },
      councilRo: {
        fieldName: "councilRo",
        keyName: "councilRo",
        component: "TextField",
      },
      councilRu: {
        fieldName: "councilRu",
        keyName: "councilRu",
        component: "TextField",
      },
      mayorRo: {
        fieldName: "mayorRo",
        keyName: "mayorRo",
        component: "TextField",
      },
      mayorRu: {
        fieldName: "mayorRu",
        keyName: "mayorRu",
        component: "TextField",
      },
    },
  },
  subscriptionListStatus: {
    tableHeaders: {
      nameRo: {
        colName: "nameRo",
        width: 119,
      },
      createdDate: {
        colName: "createdDate",
        width: 127,
        formatted: true,
      },
      authorFirstName: {
        colName: "authorFirstName",
        width: 157,
      },
      authorLastName: {
        colName: "authorLastName",
        width: 164,
      },
      lastModifiedDate: {
        colName: "lastModifiedDate",
        width: 135,
        formatted: true,
      },
      modificationAuthorFirstName: {
        colName: "modificationAuthorFirstName",
        width: 185,
        formatted: true,
      },
      modificationAuthorLastName: {
        colName: "modificationAuthorLastName",
        width: 201,
      },
      deleted: {
        colName: "deleted",
        width: 91,
      },
      deletedDate: {
        colName: "deletedDate",
        width: 149,
        formatted: true,
      },
      deletionAuthorFirstName: {
        colName: "deletionAuthorFirstName",
        width: 195,
      },
      deletionAuthorLastName: {
        colName: "deletionAuthorLastName",
        width: 195,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
      },
    },
  },
  genders: {
    tableHeaders: {
      name: {
        colName: "name",
        flex: 10,
      },
      description: {
        colName: "description",
        flex: 10,
      },
      deleted: {
        colName: "deleted",
        flex: 10,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      name: {
        fieldName: "name",
        keyName: "name",
        component: "TextField",
      },
      description: {
        fieldName: "description",
        keyName: "description",
        component: "TextField",
      },
      deleted: {
        fieldName: "deleted",
        keyName: "deleted",
      },
    },
  },
  subscriptionList: {
    tableHeaders: {
      nameRo: {
        colName: "nameRo",
        width: 90,
      },
      isIndependentCandidate: {
        colName: "isIndependentCandidate",
        width: 150,
      },
      idnp: {
        colName: "idnp",
        flex: 10,
      },
      dateOfBirth: {
        colName: "dateOfBirth",
        flex: 10,
        formatted: true,
      },
      professionRo: {
        colName: "professionRo",
        width: 102,
      },
      positionRo: {
        colName: "positionRo",
        flex: 8,
      },
      workPlaceRo: {
        colName: "workPlaceRo",
        flex: 10,
      },
      genderName: {
        colName: "genderName",
        flex: 8,
      },
      electionName: {
        colName: "electionName",
        flex: 10,
        isTooltip: true,
      },
      circumscriptionName: {
        colName: "circumscriptionName",
        width: 150,
        isTooltip: true,
      },
      ballotFunctionName: {
        colName: "ballotFunctionName",
        flex: 10,
      },
      politicalPartyName: {
        colName: "politicalPartyName",
        flex: 10,
        isTooltip: true,
      },
      subscriptionListStatusName: {
        colName: "subscriptionListStatusName",
        width: 140,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      lsSubscriptionListId: {
        fieldName: "lsSubscriptionListId",
        keyName: "lsSubscriptionListId",
      },
      genderId: {
        fieldName: "genderId",
        keyName: "genderId",
        component: "Autocomplete",
        action: "getGenders",
        keySelect: "genders",
      },
      electionId: {
        fieldName: "electionId",
        keyName: "electionId",
        component: "Autocomplete",
        action: "getElections",
        keySelect: "elections",
      },
      circumscriptionId: {
        fieldName: "circumscriptionId",
        keyName: "circumscriptionId",
        component: "Autocomplete",
        parent: "electionId",
        action: "getCircumscriptions",
        keySelect: "circumscriptions",
      },
      ballotFunctionId: {
        fieldName: "ballotFunctionId",
        keyName: "ballotFunctionId",
        component: "Autocomplete",
        action: "getElectionFunction",
        keySelect: "electionFunction",
      },
      politicalPartyId: {
        fieldName: "politicalPartyId",
        keyName: "politicalPartyId",
        component: "Autocomplete",
        action: "getPoliticalParties",
        keySelect: "politicalParties",
      },
      id: {
        fieldName: "id",
        keyName: "id",
      },
      subscriptionListStatusId: {
        fieldName: "subscriptionListStatusId",
        keyName: "subscriptionListStatusId",
        component: "Autocomplete",
        action: "getSubscriptionListStatus",
        keySelect: "subscriptionListStatus",
      },
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
      },
      professionRo: {
        fieldName: "professionRo",
        keyName: "professionRo",
        component: "TextField",
      },
      professionRu: {
        fieldName: "professionRu",
        keyName: "professionRu",
        component: "TextField",
      },
      positionRo: {
        fieldName: "positionRo",
        keyName: "positionRo",
        component: "TextField",
      },
      positionRu: {
        fieldName: "positionRu",
        keyName: "positionRu",
        component: "TextField",
      },
      workPlaceRo: {
        fieldName: "workPlaceRo",
        keyName: "workPlaceRo",
        component: "TextField",
      },
      workPlaceRu: {
        fieldName: "workPlaceRu",
        keyName: "workPlaceRu",
        component: "TextField",
      },
      idnp: {
        fieldName: "idnp",
        keyName: "idnp",
        component: "TextField",
      },
      dateOfBirth: {
        fieldName: "dateOfBirth",
        keyName: "dateOfBirth",
        component: "Date",
      },
      isIndependentCandidate: {
        fieldName: "isIndependentCandidate",
        keyName: "isIndependentCandidate",
        component: "Radio",
      },
    },
  },
  electionsSaise: {
    tableHeaders: {
      nameRo: {
        colName: "nameRo",
        flex: 10,
      },
      status: {
        colName: "status",
        flex: 10,
      },
      electionDate: {
        colName: "electionDate",
        flex: 10,
        formatted: true,
      },
    },
  },
  elections: {
    tableHeaders: {
      startCollectingDate: {
        colName: "startCollectingDate",
        flex: 10,
        formatted: true,
      },
      endCollectingDate: {
        colName: "endCollectingDate",
        flex: 10,
        formatted: true,
      },
      circumscriptions: {
        colName: "circumscriptions",
        flex: 10,
      },
      regions: {
        colName: "regions",
        flex: 10,
      },
      subscriptionLists: {
        colName: "subscriptionLists",
        flex: 10,
      },
      saElectionId: {
        colName: "saElectionId",
        flex: 10,
      },
      nameRo: {
        colName: "nameRo",
        flex: 10,
        isTooltip: true,
      },
      saNameRo: {
        colName: "saNameRo",
        flex: 10,
        isTooltip: true,
      },
      status: {
        colName: "status",
        flex: 10,
      },
      electionDate: {
        colName: "electionDate",
        flex: 10,
        formatted: true,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      id: {
        fieldName: "id",
        keyName: "id",
      },
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
        style: {
          gridColumn: "span 2",
        },
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
        style: {
          gridColumn: "span 2",
        },
      },

      startCollectingDate: {
        fieldName: "startCollectingDate",
        keyName: "startCollectingDate",
        component: "Date",
      },
      endCollectingDate: {
        fieldName: "endCollectingDate",
        keyName: "endCollectingDate",
        component: "Date",
      },
      electionStatusId: {
        fieldName: "electionStatusId",
        keyName: "electionStatusId",
        component: "Autocomplete",
        action: "getElectionStatus",
        keySelect: "electionStatus",
      },
    },
  },
  workFlows: {
    tableHeaders: {
      code: {
        colName: "code",
        flex: 10,
      },
      entityType: {
        colName: "entityType",
        flex: 10,
      },
      isDeleted: {
        colName: "isDeleted",
        flex: 10,
      },
      workflowEntities: {
        colName: "workflowEntities",
        flex: 10,
      },
      workflowStates: {
        colName: "workflowStates",
        flex: 10,
        customFormat: true,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
  },
  workFlowsTransition: {
    tableHeaders: {
      stateName: {
        colName: "stateName",
        flex: 10,
      },
      requiredClaims: {
        colName: "requiredClaims",
        flex: 10,
        customField: true,
      },
      rolesToNotify: {
        colName: "rolesToNotify",
        flex: 10,
        customField: true,
      },
      toStates: {
        colName: "toStates",
        flex: 10,
        customField: true,
      },
    },
  },
  workFlowsStatus: {
    tableHeaders: {
      code: {
        colName: "code",
        flex: 10,
      },
      isDeleted: {
        colName: "isDeleted",
        flex: 10,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      code: {
        fieldName: "code",
        keyName: "code",
        component: "TextField",
      },
    },
  },
  politicalParties: {
    tableHeaders: {
      code: {
        colName: "code",
        width: 190,
      },
      createdDate: {
        colName: "createdDate",
        width: 190,
        formatted: true,
      },
      deleted: {
        colName: "deleted",
        width: 190,
      },
      deletedDate: {
        colName: "deletedDate",
        width: 190,
        formatted: true,
      },
      isElectoralBlocK: {
        colName: "isElectoralBlocK",
        width: 190,
      },
      lastModifiedDate: {
        colName: "lastModifiedDate",
        width: 190,
        formatted: true,
      },
      nameRo: {
        colName: "nameRo",
        width: 198,
        isTooltip: true,
      },
      saPoliticalPartyId: {
        colName: "saPoliticalPartyId",
        width: 190,
      },
      shortNameRo: {
        colName: "shortNameRo",
        width: 190,
      },
      isAction: {
        colName: "isAction",
      },
    },
    formFields: {
      id: {
        fieldName: "id",
        keyName: "id",
      },
      saPoliticalPartyId: {
        fieldName: "saPoliticalPartyId",
        keyName: "saPoliticalPartyId",
        component: "TextField",
      },
      code: {
        fieldName: "code",
        keyName: "code",
        component: "TextField",
      },
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
        style: {
          gridColumn: "span 2",
        },
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
        style: {
          gridColumn: "span 2",
        },
      },
      shortNameRo: {
        fieldName: "shortNameRo",
        keyName: "shortNameRo",
        component: "TextField",
      },
      isElectoralBlocK: {
        fieldName: "isElectoralBlocK",
        keyName: "isElectoralBlocK",
        component: "Radio",
      },
    },
  },
  electionTypes: {
    tableHeaders: {
      nameRo: {
        colName: "nameRo",
        flex: 10,
        isTooltip: true,
      },
      createdDate: {
        colName: "createdDate",
        flex: 10,
        formatted: true,
      },
      lastModifiedDate: {
        colName: "lastModifiedDate",
        flex: 10,
        formatted: true,
      },
      deleted: {
        colName: "deleted",
        flex: 10,
      },
      deletedBy: {
        colName: "deletedBy",
        flex: 10,
        customFormat: true,
      },
      deletedDate: {
        colName: "deletedDate",
        flex: 10,
        formatted: true,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      id: {
        fieldName: "id",
        keyName: "id",
      },
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
        style: {
          gridColumn: "span 2",
        },
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
        style: {
          gridColumn: "span 2",
        },
      },
    },
  },
  electionStatus: {
    tableHeaders: {
      nameRo: {
        colName: "nameRo",
        flex: 10,
      },
      createdDate: {
        colName: "createdDate",
        flex: 10,
        formatted: true,
      },
      // createdBy: {
      //   colName: "createdBy",
      //   flex: 10,
      // },
      lastModifiedDate: {
        colName: "lastModifiedDate",
        flex: 10,
        formatted: true,
      },
      // lastModifiedBy: {
      //   colName: "lastModifiedBy",
      //   flex: 10,
      // },
      deleted: {
        colName: "deleted",
        flex: 10,
      },
      // deletedBy: {
      //   colName: "deletedBy",
      //   flex: 10,
      // },
      deletedDate: {
        colName: "deletedDate",
        flex: 10,
        formatted: true,
      },
      isAction: {
        colName: "isAction",
        flex: 1,
      },
    },
    formFields: {
      id: {
        fieldName: "id",
        keyName: "id",
      },
      nameRo: {
        fieldName: "nameRo",
        keyName: "nameRo",
        component: "TextField",
      },
      nameRu: {
        fieldName: "nameRu",
        keyName: "nameRu",
        component: "TextField",
      },
    },
  },
};
