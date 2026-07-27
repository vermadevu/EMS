import { Component, computed, input } from '@angular/core';
import { DashboardWidget } from '../../models/dashboard-widget';
import { StatisticWidget } from '../../models/widgets/statistic-widget';

@Component({
  selector: 'app-statistic-card',
  imports: [],
  templateUrl: './statistic-card.html',
  styleUrl: './statistic-card.css',
})
export class StatisticCard {
   readonly widget = input.required<DashboardWidget>();

    readonly data = computed(() =>
        this.widget().data as StatisticWidget
    );

}
