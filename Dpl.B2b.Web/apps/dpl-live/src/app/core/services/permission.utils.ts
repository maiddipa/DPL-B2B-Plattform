import { PermissionResourceType, ResourceAction } from './dpl-api-services';

export function generatePermissionKey(
  action: ResourceAction,
  resource?: PermissionResourceType,
  referenceId?: number
) {
  if (!resource && !referenceId) {
    return action;
  }
  return `${resource}|${action}|${referenceId}`;
}
