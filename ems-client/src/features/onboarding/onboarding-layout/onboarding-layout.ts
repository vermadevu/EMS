import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { OnboardingHeaderComponent } from '../onboarding-header/onboarding-header';
import { OnboardingSidebarComponent } from '../onboarding-sidebar/onboarding-sidebar';
import { OnboardingStep } from '../../../core/models/onboarding-step';
import { OnboardingService } from '../../../core/services/onboarding-service';

@Component({
  selector: 'app-onboarding-layout',
  imports: [
    RouterOutlet,
    OnboardingHeaderComponent,
    OnboardingSidebarComponent
  ],
  templateUrl: './onboarding-layout.html',
  styleUrl: './onboarding-layout.css'
})
export class OnboardingLayoutComponent {

  private readonly onboardingService = inject(OnboardingService);
  readonly steps = this.onboardingService.steps;
}