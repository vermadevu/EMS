import { Component, input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

import { OnboardingStep } from '../../../core/models/onboarding-step';

@Component({
  selector: 'app-onboarding-sidebar',
  imports: [
    RouterLink,
    RouterLinkActive,
    MatIconModule
  ],
  templateUrl: './onboarding-sidebar.html',
  styleUrl: './onboarding-sidebar.css'
})
export class OnboardingSidebarComponent {
  readonly steps = input.required<OnboardingStep[]>();
}