import { useRouter } from "next/router";
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
} from "@mui/material";
import { ConditionalTooltip } from "~/components";
import { getRoutePath, testIds } from "~/shared";
import { INavigationDrawerLink } from "~/core";

const defaultMixin = (theme: Theme): CSSObject => ({
  height: `calc(100% - ${theme.spacing(8)})`,
  top: theme.spacing(8),
  [theme.breakpoints.down("sm")]: {
    height: `calc(100% - ${theme.spacing(6)})`,
    top: theme.spacing(6),
  },
});

const openedMixin = (theme: Theme): CSSObject => ({
  width: 240,
  transition: theme.transitions.create("width", {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.enteringScreen,
  }),
  overflowX: "hidden",
  ...defaultMixin(theme),
});

const closedMixin = (theme: Theme): CSSObject => ({
  transition: theme.transitions.create("width", {
    easing: theme.transitions.easing.sharp,
    duration: theme.transitions.duration.leavingScreen,
  }),
  overflowX: "hidden",
  width: `calc(${theme.spacing(7)} + 1px)`,
  [theme.breakpoints.up("sm")]: {
    width: `calc(${theme.spacing(9)} + 1px)`,
  },
  ...defaultMixin(theme),
});

const StyledDrawer = styled(MuiDrawer, { shouldForwardProp: prop => prop !== "open" })(({ theme, open }) => ({
  width: 240,
  flexShrink: 0,
  whiteSpace: "nowrap",
  boxSizing: "border-box",
  zIndex: theme.zIndex.appBar - 1,
  ...(open && {
    ...openedMixin(theme),
    "& .MuiDrawer-paper": openedMixin(theme),
  }),
  ...(!open && {
    ...closedMixin(theme),
    "& .MuiDrawer-paper": closedMixin(theme),
  }),
  "& .MuiList-root": {
    "& li": {
      padding: 0,
      "& > *": {
        padding: theme.spacing(1, 3),
        "& .MuiListItemText-root": {
          marginLeft: theme.spacing(3),
        },
      },
    },
  },
}));

interface INavigationDrawerProps extends DrawerProps {
  links: INavigationDrawerLink[];
}

const Drawer: React.FC<INavigationDrawerProps> = props => {
  const { open, links = [] } = props;
  const { push } = useRouter();

  return (
    <StyledDrawer {...props}>
      <List>
        {links.map(link => (
          <ListItem key={link.title}>
            <ConditionalTooltip title={link.title} condition={!open} placement="right">
              <ListItemButton
                data-testid={testIds.components.navigationDrawer.navigationLink}
                onClick={() => push(getRoutePath(link.route))}
              >
                <link.icon />
                <ListItemText primary={link.title} />
              </ListItemButton>
            </ConditionalTooltip>
          </ListItem>
        ))}
      </List>
    </StyledDrawer>
  );
};

export default Drawer;
