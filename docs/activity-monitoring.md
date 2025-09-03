# Activity Monitoring and Focus Metric

See the overarching [AGENTS plan](../AGENTS.md). This document covers capturing user activity and deriving a focus metric.

## Objectives
- Capture keyboard and mouse events.
- Combine activity frequency with timer status to compute a focus score.

## Action Steps
1. Hook global keyboard and mouse listeners.
2. Record events alongside timer phases.
3. Calculate a weighted metric representing focus during each session.
4. Store historical metrics for analysis.

## Challenges
- Respecting user privacy and OS security restrictions.
- Normalizing activity levels across different work styles.
