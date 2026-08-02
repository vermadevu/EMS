import {
  Component,
  inject,
  OnInit,
  signal
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { EmployeeService } from '../../../core/services/employee-service';
import { NotificationService } from '../../../core/services/notification-service';

import { UpdateProfile } from '../../../core/models/update-profile';
import { EmployeeProfile } from '../../../core/models/employee-profile';
import { DetailItemComponent } from '../../../shared/components/detail-item/detail-item';
import { DatePipe } from '@angular/common';
import { OnboardingService } from '../../../core/services/onboarding-service';

@Component({
  selector: 'app-profile-step',
  imports: [
    ReactiveFormsModule,
    DetailItemComponent,
    DatePipe
  ],
  templateUrl: './profile-step.html',
  styleUrl: './profile-step.css'
})
export class ProfileStepComponent implements OnInit {
  readonly loading = signal(false);
  private readonly fb = inject(FormBuilder);
  private readonly employeeService = inject(EmployeeService);
  private readonly notificationService = inject(NotificationService);
  private readonly onboardingService = inject(OnboardingService);
  readonly profile = signal<EmployeeProfile | null>(null);

  private readonly router = inject(Router);

  readonly form = this.fb.nonNullable.group({
    address: ['', Validators.required],
    city: ['', Validators.required],
    state: ['', Validators.required],
    country: ['India', Validators.required],
    dateOfBirth: [''],
    gender: [''],
    bloodGroup: [''],
    emergencyContactName: ['', Validators.required],
    emergencyContactPhone: ['', Validators.required],
    emergencyContactRelationship: ['', Validators.required]
  });

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {
    this.loading.set(true);
    this.employeeService
      .getMyProfile()
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: profile => {
          this.profile.set(profile);
          this.patchProfile(profile);
        }
      });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    this.employeeService
      .updateMyProfile(
        this.form.getRawValue() as UpdateProfile
      )
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {
          this.onboardingService.complete(
            '/onboarding/profile'
          );

          this.notificationService.success(
            'Profile updated successfully.'
          );

          this.router.navigate([
            '/onboarding/documents'
          ]);

        }
      });
  }

  private patchProfile(profile: EmployeeProfile): void {
    this.form.patchValue({
      address: profile.address ?? '',
      city: profile.city ?? '',
      state: profile.state ?? '',
      country: profile.country ?? 'India',
      dateOfBirth: profile.dateOfBirth ?? '',
      gender: profile.gender ?? '',
      bloodGroup: profile.bloodGroup ?? '',
      emergencyContactName: profile.emergencyContactName ?? '',
      emergencyContactPhone: profile.emergencyContactPhone ?? '',
      emergencyContactRelationship: profile.emergencyContactRelationship ?? ''
    });
  }
}