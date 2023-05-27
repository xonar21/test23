import { GetServerSideProps, NextPage } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import i18nConfig from "@i18n";
import { useDispatch, useSelector } from "react-redux";
import { VoterSelectors, VoterActions } from "~/store";
import { useEffect, useState } from "react";
import { useTranslation } from "next-i18next";
import TabBar from "~/components/tab-bar";
import { defaultConfig } from "~/configs/page-config";
import UniversalTable from "~/components/wrapper-page";
import { IVoterProfilePreview } from "~/models";
import PersonalData from "~/components/personal-data";
import SubscribeToNotification from "~/components/SubscribeToNotification";
import { useRouter } from "next/router";
import SuccessModal from "~/components/successModal";
import ReactDOM from "react-dom";
import React from "react";

const Subscriptions: NextPage = () => {
  const dispatch = useDispatch();
  const router = useRouter();
  const { t } = useTranslation(["common"]);
  const { data } = useSelector(VoterSelectors.getRoot);
  const [dataProfile, setDataProfile] = useState<IVoterProfilePreview>({
    rsaPersonId: "",
    idnp: "",
    registrationDate: "",
    email: "",
    lastName: "",
    firstName: "",
    middleName: "",
    dateOfBirth: "",
    genderId: "",
    genderName: "",
    identityNumber: "",
    identitySeries: "",
    personStatusId: "",
    personStatusName: "",
    revision: "",
    disactivationDate: "",
    regionId: "",
    regionName: "",
    localityId: "",
    localityName: "",
    street: "",
    house: "",
    bloc: "",
    apartment: "",
  });

  const showSuccessModal = (title: string, message: string) => {
    const successModal = document.createElement("div");
    document.body.appendChild(successModal);

    ReactDOM.render(React.createElement(SuccessModal, { title, message }), successModal);
  };

  useEffect(() => {
    dispatch(VoterActions.getVoterProfileOwn());
  }, []);
  useEffect(() => {
    setDataProfile(data);
  }, [data]);

  useEffect(() => {
    const match = router.asPath.match(/token=([^&]*)/);

    const token = match ? match[1] : "";

    const fetchData = async () => {
      const response = await dispatch(
        VoterActions.postConfirmAttached({
          email: router.query.email,
          idnp: router.query.idnp,
          token: token,
        }),
      );

      if (response.succeeded) {
        showSuccessModal("Succes", "Mail pentru notificări adăugat cu succes");
      }
    };

    fetchData();
  }, []);

  const votersAddressRowDataPath = (res: any) => {
    return res.payload.items;
  };

  const paramsRequest = (number?: number, size?: number, filters?: object, sortField?: string, sortOrder?: string) => {
    return {
      number,
      size,
      filters,
      sortField,
      sortOrder,
    };
  };
  const tabs = [
    {
      label: t("dataPersonal"),
      content: <PersonalData dataProfile={dataProfile} />,
    },
    {
      key: "address",
      label: t("addresses"),
      content: (
        <UniversalTable<IVoterProfilePreview, any>
          key={"addresses"}
          getData={VoterActions.getVoterProfileOwnAdress}
          tableConfig={defaultConfig.voterProfileAddress}
          modifyRowDataPath={votersAddressRowDataPath}
          paramsRequest={paramsRequest}
        />
      ),
    },
    {
      key: "person",
      label: t("persons"),
      content: (
        <UniversalTable<IVoterProfilePreview, any>
          key={"persons"}
          getData={VoterActions.getVoterProfileOwnPerson}
          tableConfig={defaultConfig.voterProfilePerson}
          modifyRowDataPath={votersAddressRowDataPath}
          paramsRequest={paramsRequest}
        />
      ),
    },
    {
      key: "subscribeToNotification",
      label: t("subscribeToNotification"),
      content: (
        <>
          <SubscribeToNotification />
        </>
      ),
    },
  ];
  return (
    <>
      <TabBar items={tabs} />
    </>
  );
};

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};

export default Subscriptions;
