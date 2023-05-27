import Image from "next/image";
import {
  Drawer as MuiDrawer,
  Theme,
  CSSObject,
  styled,
  DrawerProps,
  List,
  ListItemButton,
  ListItemText,
  ListItem,
  Box,
  IconButton,
  Typography,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Link,
} from "@mui/material";
import NextLink from "next/link";
import { Reorder as ReorderIcon } from "@mui/icons-material";
import { ConditionalTooltip } from "~/components";
import { getRoutePath, testIds } from "~/shared";
import { INavigationDrawerLink } from "~/core";
import { useTranslation } from "next-i18next";
import { useRouter } from "next/router";

interface INavigationDrawerProps extends DrawerProps {
  links: INavigationDrawerLink[];
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

const Drawer: React.FC<INavigationDrawerProps> = props => {
  const { open, setOpen, links = [], ...rest } = props;
  const { t } = useTranslation(["common"]);
  const router = useRouter();

  const wrapNextLink = (link: INavigationDrawerLink, children: React.ReactElement) => {
    if (link.route) {
      return (
        <Box component="div" width="100%">
          <NextLink href={getRoutePath(link.route)} passHref>
            {children}
          </NextLink>
        </Box>
      );
    }

    return children;
  };

  const listItemButton = (link: INavigationDrawerLink, level: number) =>
    wrapNextLink(
      link,
      <Link
        component={ListItemButton}
        data-testid={testIds.components.navigationDrawer.navigationLink}
        sx={{
          color: ({ palette }) => palette.common.white,
          padding: ({ spacing }) => (open ? spacing(1.25, 1, 1.25, 2.5 * level) : spacing(1.25)),
          ...(!open && { justifyContent: "center" }),
          backgroundColor: link.route?.path === router.pathname ? "rgba(0, 64, 122, 0.6)" : "transparent",
        }}
      >
        <link.icon sx={{ width: 20, height: 20 }} />
        {open && <ListItemText primary={link.title} sx={{ m: ({ spacing }) => spacing(0, 0, 0, 0.625) }} />}
      </Link>,
    );

  const listItem = (link: INavigationDrawerLink, level = 1) => (
    <ListItem key={link.title} sx={{ p: 0 }}>
      {link.children?.length ? (
        <Accordion
          sx={{
            color: ({ palette }) => palette.common.white,
            padding: 0,
            width: "100%",
            boxShadow: "none",
          }}
        >
          <ConditionalTooltip title={link.title} condition={!open} placement="right">
            <AccordionSummary>{listItemButton(link, level)}</AccordionSummary>
          </ConditionalTooltip>
          <AccordionDetails>
            <List sx={{ p: 0 }}>
              {link.children.map(child => {
                if (child.display === false) {
                  return;
                }

                return listItem(child, level + 1);
              })}
            </List>
          </AccordionDetails>
        </Accordion>
      ) : (
        <ConditionalTooltip title={link.title} condition={!open} placement="right">
          {listItemButton(link, level)}
        </ConditionalTooltip>
      )}
    </ListItem>
  );

  return (
    <StyledDrawer
      open={open}
      PaperProps={{
        sx: {
          backgroundColor: ({ palette }) => palette.primary.main,
          backgroundImage: "url(/svg/traditional-pattern-alpha.svg)",
          backgroundRepeat: "no-repeat",
          backgroundPositionX: 5,
        },
      }}
      {...rest}
    >
      <Box
        sx={{
          color: ({ palette }) => palette.common.white,
          display: "flex",
          alignItems: "center",
          flexDirection: open ? "row" : "column",
          mt: open ? 2.75 : 3.75,
          ...(open && { ml: 2.5, mr: 1.5 }),
        }}
      >
        <IconButton
          disableRipple
          color="inherit"
          aria-label="menu"
          onClick={() => setOpen(prevState => !prevState)}
          data-testid={testIds.components.header.buttons.drawerToggler}
          sx={({ palette }) => ({
            width: 20,
            height: 20,
            padding: 0,
            borderRadius: 0,
            "&:focus-visible": {
              boxShadow: `0 0 0 2px ${palette.common.white}`,
            },
            ...(open && { mr: 1.875, transform: "rotate(90deg)" }),
            ...(!open && { mb: 2.5 }),
          })}
        >
          <ReorderIcon sx={{ width: 20, height: 20 }} />
        </IconButton>
        <Image src="/svg/cec-logo.svg" alt="CEC Logo" width={35} height={35} />
        {open && (
          <Typography variant="subtitle1" sx={{ ml: 0.625, whiteSpace: "normal", maxWidth: 152 }}>
            {t("brandName")}
          </Typography>
        )}
      </Box>
      <List
        sx={{ pt: open ? 10.375 : 4.375, maxHeight: "calc(100vh - 35px)", overflowY: "overlay", overflowX: "hidden" }}
      >
        {links.map(link => {
          if (link.display) {
            return listItem(link);
          }
        })}
      </List>
    </StyledDrawer>
  );
};

const openedMixin = ({ spacing }: Theme): CSSObject => ({
  width: spacing(32.5),
  overflowX: "hidden",
});

const closedMixin = ({ spacing }: Theme): CSSObject => ({
  width: spacing(10.5),
  overflowX: "hidden",
});

const StyledDrawer = styled(MuiDrawer, { shouldForwardProp: prop => prop !== "open" })(({ theme, open }) => ({
  width: theme.spacing(32.5),
  flexShrink: 0,
  whiteSpace: "nowrap",
  boxSizing: "border-box",
  zIndex: theme.zIndex.appBar + 1,
  height: "100%",
  ...(open && {
    ...openedMixin(theme),
    "& .MuiDrawer-paper": openedMixin(theme),
  }),
  ...(!open && {
    ...closedMixin(theme),
    "& .MuiDrawer-paper": closedMixin(theme),
  }),
  "& .MuiList-root": {
    color: theme.palette.common.white,
    "& li": {
      "& > .MuiAccordion-root": {
        background: "transparent",
        "& > div": {
          padding: 0,
          minHeight: "unset",
          "& > div": {
            margin: 0,
          },
        },
        "& .MuiAccordionDetails-root": {
          padding: 0,
        },
      },
    },
    "&::-webkit-scrollbar": {
      width: 4,
    },
    "&::-webkit-scrollbar-thumb": {
      backgroundColor: theme.palette.common.white,
      borderRadius: 3,
    },
    "&::-webkit-scrollbar-track": {
      backgroundColor: "transparent",
    },
  },
}));

export default Drawer;
