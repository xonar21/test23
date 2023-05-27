import React from "react";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  FormControl,
  FormControlLabel,
  FormHelperText,
  FormLabel,
  OutlinedInput,
  Radio,
  RadioGroup,
  Typography,
} from "@mui/material";
import { IVoterProfilePreview } from "~/models";
import { useTranslation } from "next-i18next";
import dayjs from "dayjs";
import { EventOutlined, ExpandMore, ExpandMoreOutlined } from "@mui/icons-material";

interface PersonalDataProps {
  dataProfile: IVoterProfilePreview;
}

const PersonalData: React.FC<PersonalDataProps> = ({ dataProfile }) => {
  const { t } = useTranslation(["common"]);
  const [expanded, setExpanded] = React.useState<{ [key: string]: boolean }>({});

  const handleChange = (panel: string) => (event: React.SyntheticEvent, isExpanded: boolean) => {
    setExpanded(prevState => ({ ...prevState, [panel]: isExpanded }));
  };

  const formatDate = (date: string) => {
    if (date) {
      return dayjs(date).format("DD-MM-YYYY");
    }
    return "";
  };

  return (
    <>
      <Accordion
        expanded={expanded["panel1"] || false}
        onChange={handleChange("panel1")}
        sx={{
          border: "0.5px solid #002E6F",
          borderRadius: "0px",
          borderTopLeftRadius: "0px !important",
          borderTopRightRadius: "0px !important",
          boxShadow: "none",
          ".MuiButtonBase-root": {
            flexDirection: "row-reverse",
            padding: "0",
            minHeight: "auto !important",
            paddingTop: "10px",
            paddingLeft: "5px",
            paddingRight: "5px",
            marginBottom: "10px",
          },
          ".MuiAccordionSummary-content": { margin: "0 !important" },
        }}
      >
        <AccordionSummary
          expandIcon={<ExpandMore sx={{ fontSize: "20px", color: "#00305C" }} />}
          aria-controls="panel1d-content"
          id="panel1d-header"
          sx={{ margin: "0", marginBottom: "13px" }}
        >
          <Typography
            sx={{ marginLeft: "5px", color: "#00305C", fontSize: "12px", lineHeight: "14px", fontWeight: "bold" }}
          >
            {t("basicInformation")}
          </Typography>
        </AccordionSummary>
        <AccordionDetails sx={{ padding: "0", paddingLeft: "31px", paddingRight: "31px", paddingBottom: "20px" }}>
          <Typography>
            <Box
              sx={{ display: "grid", gridTemplateColumns: "repeat(5, max-content)", gap: "10px", alignItems: "end" }}
            >
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("firstName")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "264px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.firstName}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("lastName")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "264px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.lastName}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("middleName")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "264px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.middleName}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("dateOfBirth")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "178px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={formatDate(dataProfile?.dateOfBirth)}
                  endAdornment={<EventOutlined sx={{ color: "#00305C", marginRight: "12px" }} />}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("genderName")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "164px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.genderName}
                />
              </FormControl>
            </Box>
          </Typography>
        </AccordionDetails>
      </Accordion>

      <Accordion
        expanded={expanded["panel2"] || false}
        onChange={handleChange("panel2")}
        sx={{
          border: "0.5px solid #002E6F",
          borderRadius: "0px",
          borderTopLeftRadius: "0px !important",
          borderTopRightRadius: "0px !important",
          boxShadow: "none",
          marginTop: "20px",
          ".MuiButtonBase-root": {
            flexDirection: "row-reverse",
            padding: "0",
            minHeight: "auto !important",
            paddingTop: "10px",
            paddingLeft: "5px",
            paddingRight: "5px",
            marginBottom: "10px",
          },
          ".MuiAccordionSummary-content": { margin: "0 !important" },
        }}
      >
        <AccordionSummary
          expandIcon={<ExpandMore sx={{ fontSize: "20px", color: "#00305C" }} />}
          aria-controls="panel1d-content"
          id="panel1d-header"
          sx={{ margin: "0", marginBottom: "13px" }}
        >
          <Typography
            sx={{ marginLeft: "5px", color: "#00305C", fontSize: "12px", lineHeight: "14px", fontWeight: "bold" }}
          >
            {t("identificationData")}
          </Typography>
        </AccordionSummary>
        <AccordionDetails sx={{ padding: "0", paddingLeft: "31px", paddingRight: "31px", paddingBottom: "20px" }}>
          <Typography>
            <Box
              sx={{ display: "grid", gridTemplateColumns: "repeat(5, max-content)", gap: "10px", alignItems: "end" }}
            >
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("rsaPersonId")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "264px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.rsaPersonId}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("idnp")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "264px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.idnp}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("identitySeries")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "164px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.identitySeries}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("identityNumber")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "264px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.identityNumber}
                />
              </FormControl>
            </Box>
          </Typography>
        </AccordionDetails>
      </Accordion>

      <Accordion
        expanded={expanded["panel3"] || false}
        onChange={handleChange("panel3")}
        sx={{
          border: "0.5px solid #002E6F",
          borderRadius: "0px",
          borderTopLeftRadius: "0px !important",
          borderTopRightRadius: "0px !important",
          boxShadow: "none",
          marginTop: "20px",
          ".MuiButtonBase-root": {
            flexDirection: "row-reverse",
            padding: "0",
            minHeight: "auto !important",
            paddingTop: "10px",
            paddingLeft: "5px",
            paddingRight: "5px",
            marginBottom: "10px",
          },
          ".MuiAccordionSummary-content": { margin: "0 !important" },
        }}
      >
        <AccordionSummary
          expandIcon={<ExpandMore sx={{ fontSize: "20px", color: "#00305C" }} />}
          aria-controls="panel1d-content"
          id="panel1d-header"
          sx={{ margin: "0", marginBottom: "13px" }}
        >
          <Typography
            sx={{ marginLeft: "5px", color: "#00305C", fontSize: "12px", lineHeight: "14px", fontWeight: "bold" }}
          >
            {t("address")}
          </Typography>
        </AccordionSummary>
        <AccordionDetails sx={{ padding: "0", paddingLeft: "31px", paddingRight: "31px", paddingBottom: "20px" }}>
          <Typography>
            <Box
              sx={{ display: "grid", gridTemplateColumns: "repeat(6, max-content)", gap: "10px", alignItems: "end" }}
            >
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("regionName")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "300px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.regionName}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("localityName")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "300px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.localityName}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("street")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "300px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.street}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("bloc")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "80px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.bloc}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("house")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "80px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.house}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("apartment")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "80px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.apartment}
                />
              </FormControl>
            </Box>
          </Typography>
        </AccordionDetails>
      </Accordion>

      <Accordion
        expanded={expanded["panel4"] || false}
        onChange={handleChange("panel4")}
        sx={{
          border: "0.5px solid #002E6F",
          borderRadius: "0px",
          borderTopLeftRadius: "0px !important",
          borderTopRightRadius: "0px !important",
          boxShadow: "none",
          marginTop: "20px",
          ".MuiButtonBase-root": {
            flexDirection: "row-reverse",
            padding: "0",
            minHeight: "auto !important",
            paddingTop: "10px",
            paddingLeft: "5px",
            paddingRight: "5px",
            marginBottom: "10px",
          },
          ".MuiAccordionSummary-content": { margin: "0 !important" },
        }}
      >
        <AccordionSummary
          expandIcon={<ExpandMore sx={{ fontSize: "20px", color: "#00305C" }} />}
          aria-controls="panel1d-content"
          id="panel1d-header"
          sx={{ margin: "0", marginBottom: "13px" }}
        >
          <Typography
            sx={{ marginLeft: "5px", color: "#00305C", fontSize: "12px", lineHeight: "14px", fontWeight: "bold" }}
          >
            {t("status")}
          </Typography>
        </AccordionSummary>
        <AccordionDetails sx={{ padding: "0", paddingLeft: "31px", paddingRight: "31px", paddingBottom: "20px" }}>
          <Typography>
            <Box
              sx={{ display: "grid", gridTemplateColumns: "repeat(6, max-content)", gap: "10px", alignItems: "end" }}
            >
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("registrationDate")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "178px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={formatDate(dataProfile?.registrationDate)}
                  endAdornment={<EventOutlined sx={{ color: "#00305C", marginRight: "12px" }} />}
                />
              </FormControl>

              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("disactivationDate")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "178px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={formatDate(dataProfile?.disactivationDate)}
                  endAdornment={<EventOutlined sx={{ color: "#00305C", marginRight: "12px" }} />}
                />
              </FormControl>
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("revision")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "80px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.revision}
                />
              </FormControl>

              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("personStatusName")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "178px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.personStatusName}
                  endAdornment={<ExpandMoreOutlined sx={{ color: "#00305C", marginRight: "12px" }} />}
                />
              </FormControl>
            </Box>
          </Typography>
        </AccordionDetails>
      </Accordion>

      <Accordion
        expanded={expanded["panel5"] || false}
        onChange={handleChange("panel5")}
        sx={{
          border: "0.5px solid #002E6F",
          borderRadius: "0px",
          borderTopLeftRadius: "0px !important",
          borderTopRightRadius: "0px !important",
          boxShadow: "none",
          marginTop: "20px",
          ".MuiButtonBase-root": {
            flexDirection: "row-reverse",
            padding: "0",
            minHeight: "auto !important",
            paddingTop: "10px",
            paddingLeft: "5px",
            paddingRight: "5px",
            marginBottom: "10px",
          },
          ".MuiAccordionSummary-content": { margin: "0 !important" },
        }}
      >
        <AccordionSummary
          expandIcon={<ExpandMore sx={{ fontSize: "20px", color: "#00305C" }} />}
          aria-controls="panel1d-content"
          id="panel1d-header"
          sx={{ margin: "0", marginBottom: "13px" }}
        >
          <Typography
            sx={{ marginLeft: "5px", color: "#00305C", fontSize: "12px", lineHeight: "14px", fontWeight: "bold" }}
          >
            {t("contactInfo")}
          </Typography>
        </AccordionSummary>
        <AccordionDetails sx={{ padding: "0", paddingLeft: "31px", paddingRight: "31px", paddingBottom: "20px" }}>
          <Typography>
            <Box
              sx={{ display: "grid", gridTemplateColumns: "repeat(6, max-content)", gap: "10px", alignItems: "end" }}
            >
              <FormControl sx={{ width: "max-content" }} variant="outlined">
                <FormHelperText
                  id="outlined-weight-helper-text"
                  sx={{
                    margin: "0",
                    color: "#00305C",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    marginBottom: "5px",
                  }}
                >
                  {t("email")}
                </FormHelperText>
                <OutlinedInput
                  disabled
                  id="outlined-adornment-weight"
                  aria-describedby="outlined-weight-helper-text"
                  sx={{
                    height: "32px",
                    padding: "0px",
                    fontSize: "12px",
                    lineHeight: "14px",
                    fontWeight: "normal",
                    width: "300px",
                    "& fieldset": {
                      border: "0.5px solid #00305F",
                      borderRadius: "3px",
                      borderColor: "#00305F !important",
                    },
                  }}
                  value={dataProfile?.email}
                />
              </FormControl>
            </Box>
          </Typography>
        </AccordionDetails>
      </Accordion>

      <style>
        {`
          @keyframes fadeIn {
            0% {
              opacity: 0;
            }
            100% {
              opacity: 1;
            }
          }

          .fadeIn {
            animation: fadeIn 0.5s ease-in-out;
          }
        `}
      </style>
    </>
  );
};

export default PersonalData;
