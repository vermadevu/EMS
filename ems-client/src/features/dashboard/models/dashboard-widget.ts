import { DashboardWidgetType } from './dashboard-widget-type';

export interface DashboardWidget {
    type: DashboardWidgetType;
    title: string;
    order: number;
    width: number;
    data: unknown;
}