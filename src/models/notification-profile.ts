export interface INotificationProfileUpdate {
  id?: string;
  name: string;
  entityId: string;
  profileType: string;
  eventIds: any[];
}

export interface INotificationProfilesPreview {
  data: any;
  items?: any;
}
