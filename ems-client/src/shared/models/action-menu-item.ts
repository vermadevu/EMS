export interface ActionMenuItem {
    id: string;
    label: string;
    icon: string;
    disabled?: boolean;
    color?: 'default' | 'primary' | 'success' | 'warning' | 'error';
}