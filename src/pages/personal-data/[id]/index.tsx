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
import { useRouter } from "next/router";

const Subscriptions: NextPage = () => {
  const dispatch = useDispatch();
  const { t } = useTranslation(["common"]);
  const router = useRouter();
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

  useEffect(() => {
    dispatch(VoterActions.getVoterProfileIdnp(router.query.id as string));
  }, []);

  useEffect(() => {
    setDataProfile(data);
  }, [data]);

  const votersAddressRowDataPath = (res: any) => {
    return res.payload.items;
  };

  const paramsRequest = (number?: number, size?: number, filters?: object, sortField?: string, sortOrder?: string) => {
    return {
      idnp: router.query.id as string,
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
          getData={VoterActions.getVoterProfileOwnAdressIdnp}
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
          getData={VoterActions.getVoterProfileOwnPersonIdnp}
          tableConfig={defaultConfig.voterProfilePersonId}
          modifyRowDataPath={votersAddressRowDataPath}
          paramsRequest={paramsRequest}
        />
      ),
    },
  ];
  return (
    <>
      <TabBar items={tabs} />
    </>
  );
};
// router.query.id as string
export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};

export default Subscriptions;
