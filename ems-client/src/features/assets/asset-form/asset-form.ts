import {
  Component,
  effect,
  inject,
  input,
  output
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { AssetType } from '../../../core/models/asset-type';
import { CreateAsset } from '../../../core/models/create-asset';
import { Asset } from '../../../core/models/asset';

@Component({
  selector: 'app-asset-form',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './asset-form.html',
  styleUrl: './asset-form.css'
})
export class AssetFormComponent {
  readonly loading = input(false);
  readonly save = output<CreateAsset>();

  private readonly fb = inject(FormBuilder);
  readonly cancel = output<void>();
  readonly asset = input<Asset | null>(null);

readonly assetTypes = [
  { value: 'Laptop', label: 'Laptop' },
  { value: 'Desktop', label: 'Desktop' },
  { value: 'Monitor', label: 'Monitor' },
  { value: 'Keyboard', label: 'Keyboard' },
  { value: 'Mouse', label: 'Mouse' },
  { value: 'Headset', label: 'Headset' },
  { value: 'Phone', label: 'Phone' },
  { value: 'Tablet', label: 'Tablet' },
  { value: 'Printer', label: 'Printer' },
  { value: 'AccessCard', label: 'Access Card' },
  { value: 'IdCard', label: 'ID Card' },
  { value: 'Other', label: 'Other' }
];

  readonly form = this.fb.nonNullable.group({
    assetName: ['', Validators.required],
    assetType: ['Laptop', Validators.required],
    brand: [''],
    model: [''],
    serialNumber: [''],
    purchaseDate: ['', Validators.required]
  });

  constructor() {

    effect(() => {

      const asset = this.asset();

      if (!asset) return;
      this.form.patchValue({

        assetName: asset.assetName,
        assetType: asset.assetType,
        brand: asset.brand ?? '',
        model: asset.model ?? '',
        serialNumber: asset.serialNumber ?? '',
        purchaseDate: asset.purchaseDate

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

  cancelClick() {
    this.cancel.emit();
  }

}