import {signal} from '@angular/core';
import {Token} from '../models/token';

export const userSignal = signal<Token | null>(null);
