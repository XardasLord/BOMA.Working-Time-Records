import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { NgIf } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { filter, Subscription, take } from 'rxjs';
import { SharedModule } from '../../../shared/shared.module';
import { SettingsState } from '../../state/settings.state';
import { Settings } from '../../state/settings.action';
import { SettingModelRequest } from '../../models/settings.model';
import GetAll = Settings.GetAll;
import UpdateSettings = Settings.UpdateSettings;

@Component({
	selector: 'app-settings',
	standalone: true,
	imports: [NgIf, SharedModule],
	templateUrl: './settings.component.html',
	styleUrl: './settings.component.scss'
})
export class SettingsComponent implements OnInit, OnDestroy {
	private store = inject(Store);
	private fb = inject(FormBuilder);
	private subscription = new Subscription();

	form!: FormGroup;
	settings$ = this.store.select(SettingsState.getSettings);

	ngOnInit(): void {
		this.store.dispatch(new GetAll());

		this.form = this.fb.group({});

		this.settings$
			.pipe(
				filter((settings) => settings.length > 0),
				take(1)
			)
			.subscribe((settings) => {
				for (const setting of settings) {
					const value = this.coerceValue(setting.value, setting.type);
					this.form.addControl(setting.key, this.fb.control(value, setting.type === 'int' ? [Validators.min(0)] : []));
				}
			});
	}

	saveSettings(): void {
		if (this.form.invalid) {
			this.form.markAllAsTouched();
			return;
		}

		this.settings$.pipe(take(1)).subscribe((settings) => {
			const changedSettings = settings
				.filter((setting) => {
					const formValue = this.form.get(setting.key)?.value;
					const originalValue = this.coerceValue(setting.value, setting.type);
					return !this.valuesAreEqual(formValue, originalValue, setting.type);
				})
				.map(
					(setting): SettingModelRequest => ({
						key: setting.key,
						value: this.form.get(setting.key)?.value?.toString()
					})
				);

			if (changedSettings.length === 0) {
				console.log('Brak zmian — nie wysyłam nic.');
				return;
			}

			this.subscription.add(
				this.store.dispatch(new UpdateSettings(changedSettings)).subscribe(() => {
					this.store.dispatch(new GetAll());
				})
			);
		});
	}

	private coerceValue(value: string, type: string): any {
		switch (type) {
			case 'int':
				return parseInt(value, 10);
			case 'bool':
				return value === 'true';
			case 'json':
				try {
					return JSON.parse(value);
				} catch {
					return value;
				}
			default:
				return value;
		}
	}

	private valuesAreEqual(a: any, b: any, type: string): boolean {
		switch (type) {
			case 'int':
				return parseInt(a) === parseInt(b);
			case 'bool':
				return Boolean(a) === Boolean(b);
			case 'json':
				return JSON.stringify(a) === JSON.stringify(b);
			default:
				return a === b;
		}
	}

	ngOnDestroy() {
		this.subscription.unsubscribe();
	}
}
