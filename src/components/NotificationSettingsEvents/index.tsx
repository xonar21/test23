import { NextPage } from "next";
import { FormControlLabel, Switch, FormControl, FormGroup, Typography } from "@mui/material";
import React, { useEffect, useState } from "react";
import { useTranslation } from "next-i18next";
import { LoadingButton } from "@mui/lab";
import { useDispatch, useSelector } from "react-redux";
import {
  NotificationEventActions,
  NotificationProfileActions,
  NotificationEventSelectors,
  NotificationProfileSelectors,
  VoterSelectors,
} from "~/store";
import SuccessModal from "~/components/successModal";
import ReactDOM from "react-dom";
import ErrorModal from "../errorModal";

const NotificationSettingsEvents: NextPage = () => {
  const { t } = useTranslation(["common"]);
  const voterProfile = useSelector(VoterSelectors.getRoot);
  const [loading, setLoading] = useState(false);
  const [eventState, setEventState] = useState<any>([]);
  const [profileState, setProfileState] = useState<any>({
    id: "",
    name: "",
    entityId: "",
    profileType: "",
    eventIds: [],
  });
  const dispatch = useDispatch();
  const events: any = useSelector(NotificationEventSelectors.getRoot);
  const notificationProfile: any = useSelector(NotificationProfileSelectors.getRoot);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newEvents: any = profileState.eventIds;

    if (event.target.checked) {
      if (!newEvents.includes(event.target.name)) {
        newEvents.push(event.target.name);
      }
    } else {
      const index = newEvents.indexOf(event.target.name);
      if (index > -1) {
        newEvents.splice(index, 1);
      }
    }

    setProfileState({ ...profileState, events: newEvents });
  };

  useEffect(() => {
    dispatch(NotificationEventActions.getNotificationEvent());

    if (voterProfile.data.email === "") {
      showErrorModal("Eroare", "Pentru a configura notificări trebuie să atașați e-mailul dvs.");
      return;
    }
    dispatch(NotificationProfileActions.getNotificationProfile());
  }, []);

  useEffect(() => {
    setEventState(events.data);

    if (notificationProfile.data.id) {
      const events: any = [];

      notificationProfile.data.events.forEach((e: any) => {
        events.push(e.id);
      });

      setProfileState({
        ...profileState,
        id: notificationProfile.data.id,
        name: notificationProfile.data.name,
        entityId: notificationProfile.data.entityId,
        profileType: notificationProfile.data.entityType,
        eventIds: events,
      });
    }
  }, [events, notificationProfile]);

  const showSuccessModal = (title: string, message: string) => {
    const successModal = document.createElement("div");
    document.body.appendChild(successModal);

    ReactDOM.render(React.createElement(SuccessModal, { title, message }), successModal);
  };

  const showErrorModal = (title: string, message: string, error?: any) => {
    const errorModal = document.createElement("div");
    document.body.appendChild(errorModal);

    ReactDOM.render(React.createElement(ErrorModal, { title, message, error }), errorModal);
  };

  const handleSave = async () => {
    setLoading(true);
    try {
      const response = await dispatch(NotificationProfileActions.updateNotificationProfile(profileState));
      if (response.succeeded) {
        showSuccessModal("Succes", "Setările de notificare au fost actualizate cu succes");
      }
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <FormControl sx={{ marginBottom: "20px" }}>
        <FormGroup sx={{ display: "grid", gridTemplateColumns: "repeat(3, max-content)", gap: "10px" }}>
          {eventState.length > 0 && profileState.id
            ? events.data.map((item: any) => {
                let isChecked = false;
                profileState.eventIds.forEach((e: any) => {
                  if (e === item.id) {
                    isChecked = true;
                  }
                });
                return (
                  <FormControlLabel
                    control={<Switch checked={isChecked} onChange={handleChange} name={item.id} />}
                    label={item.name}
                  />
                );
              })
            : null}
          <LoadingButton
            type="submit"
            variant="contained"
            color="primary"
            loading={loading}
            onClick={handleSave}
            sx={{
              height: "32px",
              border: loading ? "1px solid transparent" : "1px solid #00305C",
              "&:hover": { backgroundColor: "white", color: "#00305C" },
              gridRow: "3",
              gridColumn: "3",
              width: "max-content",
              justifySelf: "end",
            }}
          >
            <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "normal" }}>{t("save")}</Typography>
          </LoadingButton>
        </FormGroup>
      </FormControl>
    </>
  );
};

export default NotificationSettingsEvents;
