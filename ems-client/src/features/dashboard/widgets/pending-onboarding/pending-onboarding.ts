import { Component, computed, input } from '@angular/core';
import { DashboardWidget } from '../../models/dashboard-widget';
import { PendingOnboardingWidget } from '../../models/widgets/pending-onboarding-widget';
import { EmployeeListCard } from '../../../../shared/components/employee-list-card/employee-list-card';

@Component({
  selector: 'app-pending-onboarding',
  imports: [EmployeeListCard],
  templateUrl: './pending-onboarding.html',
  styleUrl: './pending-onboarding.css',
})
export class PendingOnboarding {
      readonly widget = input.required<DashboardWidget>();

    readonly data = computed(() =>
        this.widget().data as PendingOnboardingWidget
    );
}
