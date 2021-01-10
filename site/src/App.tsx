import React, { ReactNode, useCallback, useEffect, useState } from 'react';
import classnames from 'classnames';
import Unity, { UnityContext } from 'react-unity-webgl';

import ElantraF from './assets/car/ElantraF.png';
import TwoCv from './assets/car/2Cv.png';

import OldStyle from './assets/rim/OldStyle.png';
import FiveSpiked from './assets/rim/FiveSpiked.png';
import MomoRevenge from './assets/rim/MomoRevenge.png';
import JeepWrangler from './assets/rim/JeepWrangler.png';

import CarPaintRed from './assets/paint/CarPaintRed.png';
import CarPaintBlack from './assets/paint/CarPaintBlack.png';
import CarPaintBlue from './assets/paint/CarPaintBlue.png';
import CarPaintPaleGreen from './assets/paint/CarPaintPaleGreen.png';

import './App.css';
import { StringParam, useQueryParam } from 'use-query-params';

interface OptionInfo {
	value: string;
	image: string;
}

type Tab = 'car' | 'rim' | 'paint';

const CONFIGURATION_OPTIONS: Record<Tab, OptionInfo[]> = {
	car: [
		{ value: 'ElantraF', image: ElantraF },
		{ value: '2Cv', image: TwoCv }
	],
	rim: [
		{ value: 'OldStyle', image: OldStyle },
		{ value: 'FiveSpiked', image: FiveSpiked },
		{ value: 'MomoRevenge', image: MomoRevenge },
		{ value: 'JeepWrangler', image: JeepWrangler }
	],
	paint: [
		{ value: 'CarPaintRed', image: CarPaintRed },
		{ value: 'CarPaintBlack', image: CarPaintBlack },
		{ value: 'CarPaintBlue', image: CarPaintBlue },
		{ value: 'CarPaintPaleGreen', image: CarPaintPaleGreen },
	],
};

function App() {
	const [isCollapsed, setIsCollapsed] = useState<boolean>(true);
	const [activeTab, setActiveTab] = useState<Tab>('car');
	const [lastAction, setLastAction] = useState<Tab | null>(null);
	const [lastActionValue, setLastActionValue] = useState<string | null>(null);
	const [unityContext, setUnityContext] = useState<UnityContext | null>(null);
	const [isUnityReady, setIsUnityReady] = useState<boolean>(false);

	// Save car configuration to url
	const [car, setCar] = useQueryParam('car', StringParam);
	const [rim, setRim] = useQueryParam('rim', StringParam);
	const [paint, setPaint] = useQueryParam('paint', StringParam);

	const toggleMenu = () => {
		setIsCollapsed((collapsed) => !collapsed);
	};

	const updateConfigAccordingToUrlParam = useCallback(() => {
		if (car) {
			changeConfiguration('car', car);
		}
		if (rim) {
			changeConfiguration('rim', rim);
		}
		if (paint) {
			changeConfiguration('paint', paint);
		}
	}, []);

	const changeConfiguration = useCallback((action: Tab, value: string) => {
		setLastAction(action);
		setLastActionValue(value);

		switch (action) {
			case 'car':
				setCar(value);
				break;

			case 'rim':
				setRim(value);
				break;

			case 'paint':
				setPaint(value);
				break;
		}
	}, [setLastAction, setLastActionValue, setCar, setRim, setPaint]);

	// Run once at startup
	useEffect(() => {
		// Init unity
		const context = new UnityContext({
			loaderUrl: "game/Build/game.loader.js",
			dataUrl: "game/Build/game.data",
			frameworkUrl: "game/Build/game.framework.js",
			codeUrl: "game/Build/game.wasm",
		});

		// Let unity tell us when it is finished loading the CarConfigManager
		context.on("ReadyForCommands", () => {
			console.log('received ReadyForCommands event from unity');
			setIsUnityReady(true);
			updateConfigAccordingToUrlParam();
			(document.getElementsByTagName('CANVAS')[0] as HTMLCanvasElement).style.opacity = '1';
		});

		setUnityContext(context);
	}, [updateConfigAccordingToUrlParam]);

	useEffect(() => {
		if (!lastAction || !lastActionValue || !unityContext || !isUnityReady) {
			return;
		}
		unityContext.send(
			'CarConfigManager',
			'ChangeConfiguration',
			`Change${capitalize(lastAction)}:${lastActionValue}`);
	}, [unityContext, lastAction, lastActionValue, isUnityReady]);

	useEffect(() => {
		if (car) {
			changeConfiguration('car', car);
		}
	}, [car, changeConfiguration]);

	useEffect(() => {
		if (rim) {
			changeConfiguration('rim', rim);
		}
	}, [rim, changeConfiguration]);

	useEffect(() => {
		if (paint) {
			changeConfiguration('paint', paint);
		}
	}, [paint, changeConfiguration]);

	const capitalize = (text: string): string => {
		return text.charAt(0).toUpperCase() + text.slice(1);
	};

	const renderMenuContent = (): ReactNode => {
		return <>
			<ul>
				{renderTabs()}
			</ul>
			<ul className="menu-options-wrapper">
				{renderOptions()}
			</ul>
		</>;
	};

	const renderTabs = () => {
		return Object.keys(CONFIGURATION_OPTIONS).map((tab) => {
			return <li
				className={classnames('menu-tab', { 'active': tab === activeTab })}
				onClick={() => setActiveTab(tab as Tab)}
				key={'tab-' + tab}
			>
				{capitalize(tab)}
			</li>;
		});
	};

	const renderOptions = () => {
		return CONFIGURATION_OPTIONS[activeTab].map((option) => {
			return <li
				className="menu-option"
				onClick={() => changeConfiguration(activeTab, option.value)}
				key={'option-' + option.value}
			>
				<img src={option.image} alt={'option ' + option.value} />
			</li>;
		});
	};

	if (!unityContext) {
		return null;
	}
	return (
		<div className="App">
			<Unity unityContext={unityContext} />
			<div className={classnames('collapsable-menu', { collapsed: isCollapsed })}>
				<div className="collapse-button" onClick={toggleMenu} />
				<div className="collapse-wrapper">{renderMenuContent()}</div>
			</div>
		</div>
	);
}

export default App;
