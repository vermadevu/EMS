import { Component, effect, inject, input, output } from '@angular/core';

import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { Designation } from '../../../core/models/designation';
import { CreateDesignation } from '../../../core/models/create-designation';

@Component({
  selector: 'app-designation-form',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './designation-form.html',
  styleUrl: './designation-form.css'
})
export class DesignationFormComponent {
  readonly loading = input(false);
  readonly designation = input<Designation | null>(null);
  readonly save = output<CreateDesignation>();
  readonly cancel = output<void>();
  private readonly fb = inject(FormBuilder);

  readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    description: ['']
  });

  constructor() {
    effect(() => {
      const designation = this.designation();
      if (!designation) {
        this.form.reset({
          name: '',
          description: ''
        });
        return;
      }
      this.form.patchValue({
        name: designation.name,
        description: designation.description ?? ''
      });
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.save.emit(this.form.getRawValue());

  }
  cancelClick(): void {
    this.cancel.emit();
  }
}