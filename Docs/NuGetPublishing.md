# Publish the NuGet library

I sign the library assemblies and NuGet packages locally with my USB code-signing
certificate, then stage the exact packages and their manifest in a draft GitHub
release. Publishing that draft starts `.github/workflows/publish-nuget.yml` and
exchanges GitHub OIDC for a temporary NuGet key. The workflow verifies and pushes
the signed assets without rebuilding them. The PowerShell Gallery flow remains
in `Module/Build/Build-Module.ps1`.

## Shared publisher version

This configuration uses the commands introduced by
[PSPublishModule #1214](https://github.com/EvotecIT/PSPublishModule/pull/1214).
The caller pins the merged workflow commit. Set the repository variable
`POWERFORGE_NUGET_PUBLISHER_VERSION` to a published three-part PowerForge.Build
version containing the new commands, and install that same version locally.

Install that same tool version locally:

```powershell
dotnet tool install --global PowerForge.Build --version <published-version>
# For an existing installation, use dotnet tool update instead.
```

## One-time setup

1. In `EvotecIT/Globalping`, create the GitHub environment `nuget-release`.
   Select **Selected branches and tags** and add exactly one **Tag** rule `v*`.
   Do not add a branch rule or other tag patterns. Configure required reviewers
   if desired. The workflow also requires the tag to resolve to a commit in
   `master`, the default branch.
2. Set the repository variable `NUGET_PUBLISHING_USER` to the actual nuget.org
   username creating the trusted-publishing policy. This is a profile name,
   not an email address or the GitHub login. That account needs active membership
   and publishing rights in the nuget.org organization **Evotec**.
3. On nuget.org, create a trusted-publishing policy with owner **Evotec**,
   repository owner **EvotecIT**, repository **Globalping**, workflow file
   **publish-nuget.yml**, and environment **nuget-release**. Limit its package
   scope to **Globalping** and permit new versions of that existing package.
   Enter the calling workflow filename only, without `.github/workflows/`.
4. Check **Evotec → Manage Organization → Edit Organization → Certificates**
   and Globalping's required-signer setting. The configured certificate has
   SHA1 thumbprint `92E95FB58EFFA6A4A75E77A33CDD6BFE6DD30F1A` and author SHA256
   fingerprint
   `D13C03BFEEC0CBF7FD0E0E7FFAF3F3D2076E62B1AF64665E806917B9191CFFDE`.
   Its subject is `Evotec Services sp. z o.o.` and it expires in December 2028.
   Local certificate metadata and an existing public package signature establish
   its identity; they do not confirm the current nuget.org registration. If it
   is absent from the relevant signing account, register the public DER `.cer`
   before the pilot. Once an account registers a certificate, affected future
   package uploads must use a registered certificate. Check this policy before
   changing an organization's registration because it affects other packages.
5. For local staging, provide `GITHUB_TOKEN` in the current process with the
   repository access needed to create a draft and upload assets. No NuGet API key
   file, repository secret or environment secret is used by this library flow.

See the [shared publishing guide](https://github.com/EvotecIT/PSPublishModule/blob/main/Docs/PowerForge.NuGetTrustedPublishing.md),
[NuGet policy setup](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing),
and [certificate registration rules](https://learn.microsoft.com/nuget/nuget-org/package-signing-certificates).

## Pilot run

Use committed source from `master` after the shared publisher and this
configuration are available. The library version is prepared as `1.0.3`:
`v1.0.2` already exists as a GitHub release tag, while the latest observed NuGet
version is `1.0.1`. Check both destinations again before selecting a new version.
Commit any version change before staging so the tag and manifest name the
intended source commit. `UpdateVersions: false` keeps staging on the committed
project version.

```powershell
# From the repository root, with the USB token available:
powerforge nuget stage --config ./project.build.json
# With a PSPublishModule version containing StageNuget, the equivalent cmdlet is:
# Invoke-ProjectBuild -ConfigPath ./project.build.json -StageNuget
```

Complete the normal USB prompts. Staging builds only the `Globalping` library,
signs assemblies before packing, author-signs and timestamps its `.nupkg` and
`.snupkg`, verifies them, and uploads them with `nuget-release-manifest.json` to
a new draft release named with `v{Version}`. An existing tag or draft is a
conflict; inspect a partial draft before deciding whether to remove it and retry.

Inspect the draft assets and manifest, then **publish the draft release**. Watch
**Publish NuGet** and approve the `nuget-release` environment if reviewers are
configured. Confirm the registry readback and the resulting Globalping version
on nuget.org. OIDC login, real USB signing, publishing and symbol ingestion need
this live pilot.

For a retry, manually run **Publish NuGet** with the same **published release
tag** selected. A dispatch on `master` is rejected. An existing package counts
as success only when its id, version and author signer match the manifest;
nuget.org adds its repository signature, so the published bytes can differ.
