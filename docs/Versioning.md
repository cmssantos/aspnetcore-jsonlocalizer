# Versioning and Release Guide

This document explains how to correctly version the project and trigger releases using Git tags and our CI/CD pipeline.

---

## Semantic Versioning (SemVer)

Format:  
```
MAJOR.MINOR.PATCH[-PRERELEASE]
```

- **MAJOR**: breaking changes (incompatible API changes)  
- **MINOR**: new backward-compatible features  
- **PATCH**: backward-compatible bug fixes  
- **PRERELEASE**: optional pre-release label (`alpha`, `beta`, `rc`, etc.)

---

## Creating Tags to Publish NuGet Packages

### Final (stable) Releases

Use tags like `v1.2.3`

```bash
git tag v1.2.3
git push origin v1.2.3
```

This triggers the release pipeline, which will build, test, pack, and publish a stable NuGet package.

### Pre-release Versions (alpha, beta, rc)

Use tags like `v1.3.0-alpha`, `v1.3.0-beta`, `v1.3.0-rc.1`

```bash
git tag v1.3.0-alpha
git push origin v1.3.0-alpha
```

The release pipeline will create a pre-release package with the given suffix and publish it to NuGet.

---

## When to Increment MAJOR, MINOR, PATCH

| Change Type               | Description                  | Tag Example      |
|---------------------------|------------------------------|------------------|
| Bugfix or small improvement | Fix typos, bug fixes          | `v1.2.4`         |
| New feature               | Add new backward-compatible feature | `v1.3.0`         |
| Breaking changes          | Change or remove public API   | `v2.0.0`         |

---

## Recommended Workflow

1. Ensure your code is ready and tested locally  
2. Decide the next version number based on the changes  
3. Create a git tag:

```bash
git tag <version>  # e.g. v1.3.0-beta
git push origin <version>
```

4. The release pipeline will automatically run and publish your NuGet package.

---

## Additional Tips

- Always prefix tags with `v` (e.g. `v1.0.0`)  
- For pre-release fixes, increment PATCH: `v1.3.0-alpha.1`, `v1.3.0-alpha.2`  
- Avoid force-overwriting tags unless necessary  
- Use semantic versioning to keep package consumers happy and informed

---

Feel free to reach out if you have any questions!
