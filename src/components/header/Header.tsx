import React, { useEffect, useMemo, useState } from "react";
import { AppBar, IconButton, InputAdornment, TextField, Toolbar, Typography, Link, Box, Badge } from "@mui/material";
import {
  AccountCircleOutlined,
  ContentPasteOutlined,
  EditNotificationsOutlined,
  EventOutlined,
  GroupOutlined,
  AssignmentIndOutlined,
  SupervisedUserCircleOutlined,
  AdminPanelSettingsOutlined,
  BuildOutlined,
  LocationCityOutlined,
  HowToVoteOutlined,
  ListAltOutlined,
  CollectionsBookmarkOutlined,
  NotificationsOutlined,
  PublicOutlined,
  AccountTreeOutlined,
  FormatListBulletedOutlined,
  LanguageOutlined,
  BallotOutlined,
  Search as SearchIcon,
  GroupsOutlined,
  HowToRegOutlined,
  PlaylistAddCheckCircleOutlined,
  WcOutlined,
  TimelineOutlined,
  SchemaOutlined,
  EmojiFlagsOutlined,
  CheckCircleOutline,
  AssignmentOutlined,
} from "@mui/icons-material";
import { useTranslation } from "next-i18next";
import NextLink from "next/link";

import { AuthButton, LocaleSwitch, NavigationDrawer } from "~/components";
import { getRoutePath, routes, testIds } from "~/shared";
import { INavigationDrawerLink } from "~/core";
import { useDispatch, useSelector } from "react-redux";
import { AuthSelectors, NotificationsActions, NotificationsSelectors } from "~/store";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";

const Header: React.FC = () => {
  const { t, i18n } = useTranslation(["common"]);
  const [open, setOpen] = useState(false);
  const [isDisplayLink, setIsDisplayLink] = useState(false);
  const { SaiseToken, token } = useSelector(AuthSelectors.getRoot);
  const count = useSelector(NotificationsSelectors.getRoot);
  const { hasPermission } = usePermissions();
  const dispatch = useDispatch();

  useEffect(() => {
    if (SaiseToken) {
      setIsDisplayLink(true);
    }
  }, [SaiseToken]);

  const date = useMemo(() => {
    return new Intl.DateTimeFormat(i18n.language, {
      weekday: "long",
      day: "2-digit",
      month: "short",
      year: "numeric",
    }).format(new Date());
  }, [i18n.language]);

  const links: INavigationDrawerLink[] = [
    {
      icon: AccountCircleOutlined,
      title: t("routes.personalData"),
      route: routes.PersonalData,
      display: !isDisplayLink,
    },
    {
      icon: ContentPasteOutlined,
      title: t("routes.subscriptions"),
      route: routes.Subscriptions,
      display: !isDisplayLink,
    },
    {
      icon: EditNotificationsOutlined,
      title: t("routes.notificationSettings"),
      route: routes.NotificationSettings,
      display: !isDisplayLink,
    },
    {
      icon: EventOutlined,
      title: t("routes.events"),
      route: routes.Events,
      display: !isDisplayLink,
    },
    // {
    //   icon: BuildOutlined,
    //   title: t("routes.administration"),
    //   display: isDisplayLink,
    //   children: [
    {
      icon: HowToVoteOutlined,
      title: t("routes.elections"),
      route: routes.Elections,
      display: isDisplayLink,
    },
    {
      icon: LocationCityOutlined,
      title: t("routes.circumscriptions"),
      route: routes.Circumscriptions,
      display: isDisplayLink,
    },
    {
      icon: ListAltOutlined,
      title: t("routes.subscriptionLists"),
      route: routes.SubscriptionLists,
      display: isDisplayLink,
    },
    {
      icon: GroupOutlined,
      title: t("routes.userManagement"),
      display: isDisplayLink,
      children: [
        {
          icon: SupervisedUserCircleOutlined,
          title: t("routes.users"),
          route: routes.UserManagement,
        },
        {
          icon: AssignmentIndOutlined,
          title: t("routes.roles"),
          route: routes.RoleManagement,
        },
        {
          icon: AdminPanelSettingsOutlined,
          title: t("routes.permissions"),
          route: routes.PermissionManagement,
        },
        {
          icon: GroupsOutlined,
          title: t("routes.voters"),
          route: routes.VoterManagement,
        },
      ],
    },
    {
      icon: CollectionsBookmarkOutlined,
      title: t("routes.nomenclatures"),
      display: isDisplayLink,
      children: [
        {
          icon: PublicOutlined,
          title: t("routes.regions"),
          display: hasPermission(Permission.ViewRegionList),
          route: routes.Regions,
        },
        {
          icon: AccountTreeOutlined,
          title: t("routes.designationSubjects"),
          route: routes.DesignationSubjects,
        },
        {
          icon: HowToRegOutlined,
          title: t("routes.electionFunction"),
          route: routes.ElectionFunction,
        },
        {
          icon: WcOutlined,
          title: t("routes.genders"),
          route: routes.Genders,
        },
        {
          icon: EmojiFlagsOutlined,
          title: t("routes.politicalParties"),
          route: routes.PoliticalParties,
        },
      ],
    },
    {
      icon: FormatListBulletedOutlined,
      title: t("routes.predefinedLists"),
      display: isDisplayLink,
      children: [
        {
          icon: LanguageOutlined,
          title: t("routes.regionTypes"),
          display: hasPermission(Permission.ViewRegionTypeList),
          route: routes.RegionTypes,
        },
        {
          icon: BallotOutlined,
          title: t("routes.electionTypes"),
          route: routes.ElectionTypes,
        },
        {
          icon: PlaylistAddCheckCircleOutlined,
          title: t("routes.subscriptionListStatus"),
          route: routes.SubscriptionListStatus,
        },
        {
          icon: CheckCircleOutline,
          title: t("routes.electionStatus"),
          route: routes.ElectionStatus,
        },
      ],
    },
    {
      icon: TimelineOutlined,
      title: t("routes.workFlow"),
      display: isDisplayLink,
      children: [
        {
          icon: SchemaOutlined,
          title: t("routes.workFlow"),
          route: routes.Workflows,
        },
      ],
    },
    {
      icon: AssignmentOutlined,
      title: t("routes.audit"),
      route: routes.Audit,
      display: isDisplayLink,
    },
    //   ],
    // },
  ];

  useEffect(() => {
    if (token && !SaiseToken) {
      dispatch(NotificationsActions.getNotificationsCount());
    }
  }, [token]);

  return (
    <>
      <AppBar
        position="fixed"
        data-testid={testIds.components.header.root}
        sx={({ palette, spacing }) => ({
          backgroundColor: palette.common.white,
          color: palette.primary.main,
          left: spacing(open ? 32.5 : 10.375),
          width: `calc(100% - ${spacing(open ? 32.5 : 10.375)})`,
        })}
      >
        <Toolbar sx={{ p: ({ spacing }) => `${spacing(0, 2.5)} !important` }}>
          <Typography variant="subtitle1" textTransform="capitalize" component="div" sx={{ mr: 5 }}>
            {date}
          </Typography>
          <TextField
            variant="standard"
            placeholder={t("searchPlaceholder")}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton edge="end">
                    <SearchIcon />
                  </IconButton>
                </InputAdornment>
              ),
              sx: ({ typography }) => ({ ...typography.subtitle1 }),
            }}
            sx={{
              width: 212,
              mr: "auto",
            }}
          />
          <NextLink href={getRoutePath(routes.Notifications)} passHref>
            <Link component={IconButton} color="primary">
              <Badge badgeContent={count.count} color="error">
                <NotificationsOutlined />
              </Badge>
            </Link>
          </NextLink>
          <Box mx={1.688}>{!SaiseToken && <LocaleSwitch />}</Box>
          <AuthButton />
        </Toolbar>
      </AppBar>
      <NavigationDrawer
        variant="permanent"
        links={links}
        open={open}
        setOpen={setOpen}
        aria-expanded={open}
        data-testid={testIds.components.navigationDrawer.root}
      />
    </>
  );
};

export default Header;
