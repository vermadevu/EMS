export const API_ENDPOINTS = {
  account: {
    login: '/account/login',
    me: '/account/me'
  },

  dashboard: '/dashboard',

  employees: '/employee',

  departments: '/department',

  designations: '/designation',

  assets: '/asset',

  documents: '/document',

  users: '/users',

  permissions: '/permission',

  rolePermissions: '/rolepermission',

  userPermissions: '/userpermission'
} as const;